using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Order;
using LatinMedia.DataLayer.Entities.Survay;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;

namespace LatinMedia.Core.Services
{
    public class OrderService : IOrderService
    {
        private LatinMediaContext _context;

        private IUserService _userService;

        public OrderService(LatinMediaContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #region Order
        public int AddOrder(string mobile, int courseId)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;

            Order order = _context.Orders
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
            var course = _context.Courses.Find(courseId);
            if (order == null) // اگر فاکتور بازی نداشت یک فاکتور جدید ایجاد کنه
            {
                order = new Order()
                {
                    IsFinaly = false,
                    OrderDate = DateTime.Now,
                    OrderSum = course.CoursePrice,
                    UserId = userId,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            CourseId = course.CourseId,
                            CoursePrice = course.CoursePrice,
                            OrderCount = 1,
                        }
                    }
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            else  // اگر فاکتور بازی را پیدا کرد
            {
                OrderDetail detail = _context.OrderDetails
                    .FirstOrDefault(d => d.OrderId == order.OrderId && d.CourseId == course.CourseId);
                if (detail != null)
                {
                    detail.OrderCount += 1;
                    _context.OrderDetails.Update(detail);
                }
                else
                {
                    detail = new OrderDetail()
                    {
                        CourseId = course.CourseId,
                        CoursePrice = course.CoursePrice,
                        OrderCount = 1,
                        OrderId = order.OrderId
                    };
                    _context.OrderDetails.Add(detail);
                }

                _context.SaveChanges();
                UpdateOrderPrice(order.OrderId);
            }

            return order.OrderId;


        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public void UpdateOrderPrice(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            order.OrderSum = _context.OrderDetails.Where(d => d.OrderId == order.OrderId)
                .Sum(d => d.CoursePrice);

            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public Order GetOrdersForUserPanel(string mobile, int orderId)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);
        }

        public bool FinalyOrder(string mobile, int orderId)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            var order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.IsFinaly)
            {
                return false;
            }

            if (_userService.BalanceWalletUser(mobile) >= order.OrderSum)
            {
                order.IsFinaly = true;
                _userService.AddWallet(new Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    Description = "فاکتور شماره # " + order.OrderId,
                    IsPay = true,
                    TypeId = 2,
                    UserId = userId,
                });
                _context.Orders.Update(order);
                foreach (var detail in order.OrderDetails)
                {
                    _context.userCourses.Add(new UserCourse()
                    {
                        CourseId = detail.CourseId,
                        UserId = userId,
                    });
                }
                _context.SaveChanges();
                return true;
            }

            return false;

        }

        void AddUserInClass(int userId , int courseId)
        {
            _context.userCourses.Add(new UserCourse()
            {
                CourseId = courseId,
                UserId = userId,
            });
        }

        public List<Order> GetUserOrders(string mobile)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public bool IsUserInCourse(string mobile, int courseId)
        {
            int userId = _userService.GetUserIdByMobile(mobile);

            return _context.UserCourses.Any(c => c.UserId == userId && c.CourseId == courseId);
        }

        public List<UserCourse> GetCoursesForDownload(string mobile)
        {
            int userId = _userService.GetUserIdByMobile(mobile);
            return _context.UserCourses
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Course)//همان select
                .ToList();
        }

        public int[] GetCoursForSurvay(string mobile)
        {
            int userId = _userService.GetUserIdByMobile(mobile);
            return _context.UserCourses
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.CourseId)//همان select
                .ToArray();
        }

        public void InsertPoll(SurveyViewModel surveyViewModel)
        {
            DateTime SurvayDate = DateTime.Now;
            Survay survay = new Survay();
            survay.UserId = surveyViewModel.UserId;
            survay.GenderId = surveyViewModel.GenderId;
            survay.FirstName = surveyViewModel.FirstName;
            survay.LastName = surveyViewModel.LastName;
            survay.Mobile = surveyViewModel.Mobile;
            survay.Age = surveyViewModel.Age;
            survay.EduId = surveyViewModel.Education;
            survay.EmpId = surveyViewModel.Employment;
            survay.Poll1 = surveyViewModel.Poll1;
            survay.Poll2 = surveyViewModel.Poll2;
            survay.Poll3 = surveyViewModel.Poll3;
            survay.Poll4 = surveyViewModel.Poll4;
            survay.Poll5 = surveyViewModel.Poll5;
            survay.Comment = surveyViewModel.Comment1;
            survay.SurvayDate = SurvayDate;
            _context.Survays.Add(survay);
            _context.SaveChanges();
            int survayId = _context.Survays.Where(s => s.UserId == survay.UserId).Select(s => s.SurvayId).First();
            foreach(TeachersForSurvay t in surveyViewModel.teachersForSurvays)
            {
                if(t.TeacherType == 1)
                {
                    SurvayDetail detail = new SurvayDetail();
                    detail.SurvayId = survayId;
                    detail.TeacherId = t.TeacherId;
                    detail.Poll1 = surveyViewModel.Poll6;
                    detail.Poll2 = surveyViewModel.Poll7;
                    detail.Poll3 = surveyViewModel.Poll8;
                    detail.Poll4 = surveyViewModel.Poll9;
                    detail.Poll5 = surveyViewModel.Poll10;
                    detail.Poll6 = surveyViewModel.Poll11;
                    detail.Comment = surveyViewModel.Comment2;
                    detail.SurvayDate = SurvayDate;
                    _context.SurvayDetails.Add(detail);
                    _context.SaveChanges();
                }
                else
                {
                    SurvayDetail detail = new SurvayDetail();
                    detail.SurvayId = survayId;
                    detail.TeacherId = t.TeacherId;
                    detail.Poll1 = surveyViewModel.Poll12;
                    detail.Poll2 = surveyViewModel.Poll13;
                    detail.Poll3 = surveyViewModel.Poll14;
                    detail.Poll4 = surveyViewModel.Poll15;
                    detail.Poll5 = surveyViewModel.Poll16;
                    detail.Poll6 = surveyViewModel.Poll17;
                    detail.Comment = surveyViewModel.Comment3;
                    detail.SurvayDate = SurvayDate;
                    _context.SurvayDetails.Add(detail);
                    _context.SaveChanges();
                }
            }
            User user = _context.Users.Where(u => u.UserId == surveyViewModel.UserId).Single();
            user.IsPolled = true;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public List<Course> GetCoursesForTeacher(string mobile)
        {
            int TeacherId = _userService.GetTeacherIdByMobile(mobile);
            return _context.Courses.Where(c => c.TeacherId == TeacherId && ((DateTime.Compare(DateTime.Now, c.CreateDate) < 0) || ((DateTime.Compare(DateTime.Now, c.CreateDate) >= 0 &&
                                    DateTime.Compare(DateTime.Now, c.EndDate) <= 0)))).ToList();
            
        }

        public List<Course> GetFinishedCoursesForTeacher(string mobile)
        {
            int TeacherId = _userService.GetTeacherIdByMobile(mobile);
            return _context.Courses.Where(c => c.TeacherId == TeacherId && DateTime.Compare(DateTime.Now, c.EndDate) > 0).ToList();

        }

        public List<Course> GetCoursesForInspector(string mobile, List<string> meetingIds)
        {
            var user = _userService.GetUserByMobile(mobile);
            int cityId = _userService.GetCityForAcademies(user.AcademyId);
            int stateId = _userService.GetStateFoAcademy(cityId);
            int[] stateAcademy = _context.Academies.Where(a => a.City.StateId == stateId).Select(a => a.AcademyId).ToArray();
            int[] curruntClass = _context.Courses.Where(c => meetingIds.Contains(c.DemoFileName)).Select(c => c.CourseId).ToArray();
            var courses = _context.Courses.Where(c => stateAcademy.Contains(c.CompanyId) && curruntClass.Contains(c.CourseId) && DateTime.Compare(DateTime.Now, c.EndDate) < 0).ToList();
            return courses;
        }

        public void SetServerForCourse(ConfigServersForCourses Class)
        {
            Course course = _context.Courses.Where(c => c.DemoFileName == Class.ClassId).Single();
            course.ServerId = Class.ServerId;
            _context.Courses.Update(course);
            _context.SaveChanges();
        }
        #endregion

        #region Discount

        public DiscountUseType UseDiscount(int orderId, string code)
        {
            var discount = _context.Discounts.SingleOrDefault(d => d.DiscountCode == code);

            if (discount == null)
                return DiscountUseType.NotFound;

            if (discount.StartDate != null && discount.StartDate > DateTime.Now)
                return DiscountUseType.ExpireDate;

            if (discount.EndDate != null && discount.EndDate < DateTime.Now)
                return DiscountUseType.ExpireDate;

            if (discount.UsableCount != null && discount.UsableCount < 1)
                return DiscountUseType.Finished;

            var order = GetOrderById(orderId);
            
            if (_context.userDiscountCodes.Any(d => d.UserId == order.UserId && d.DiscountId == discount.DiscountId))
            {
                return DiscountUseType.Used;
            }

            int percent = (order.OrderSum * discount.DiscountPercent) / 100;
            order.OrderSum = order.OrderSum - percent;
            UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }

            _context.Discounts.Update(discount);

            _context.userDiscountCodes.Add(new UserDiscountCode()
            {
                DiscountId = discount.DiscountId,
                UserId = order.UserId

            });
            _context.SaveChanges();

            return DiscountUseType.Success;
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }

        public void UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            _context.SaveChanges();
        }

        public Discount GetDiscountById(int discountId)
        {
            return _context.Discounts.Find(discountId);
        }

        public List<Discount> GetAllDiscounts()
        {
            return _context.Discounts.ToList();
        }

        public bool IsExistDiscount(string code)
        {
            return _context.Discounts.Any(d => d.DiscountCode == code.ToLower());
        }


        #endregion

        
    }
}
