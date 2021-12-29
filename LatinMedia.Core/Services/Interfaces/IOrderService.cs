using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Order;
using LatinMedia.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.Services.Interfaces
{
   public interface IOrderService
   {
        #region Order
        int AddOrder(string mobile, int courseId);
        void UpdateOrderPrice(int orderId);
        Order GetOrdersForUserPanel(string mobile, int orderId);
        Order GetOrderById(int orderId);
        bool FinalyOrder(string mobile, int orderId);
        List<Order> GetUserOrders(string mobile);
        void UpdateOrder(Order order);
        bool IsUserInCourse(string mobile, int courseId);
        List<UserCourse> GetCoursesForDownload(string mobile);
        int[] GetCoursForSurvay(string mobile);
        void InsertPoll(SurveyViewModel surveyViewModel);
        List<Course> GetCoursesForTeacher(string mobile);
        List<Course> GetFinishedCoursesForTeacher(string mobile);
        List<Course> GetCoursesForInspector(string mobile, List<string> meetingIds);
        void SetServerForCourse(ConfigServersForCourses Class);
        #endregion

        #region Discount

        DiscountUseType UseDiscount(int orderId, string code);

        void AddDiscount(Discount discount);
        void UpdateDiscount(Discount discount);
        Discount GetDiscountById(int discountId);
        List<Discount> GetAllDiscounts();
        bool IsExistDiscount(string code);

        #endregion

        
    }
}
