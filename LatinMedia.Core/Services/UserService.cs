using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Genertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Survay;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.DataLayer.Entities.Wallet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Remotion.Linq.Clauses.ResultOperators;

namespace LatinMedia.Core.Services
{
    public class UserService : IUserService
    {
        private LatinMediaContext _context;
        private IHostingEnvironment _environment;
        private IPermissionService _permissionService;

        public UserService(LatinMediaContext context, IHostingEnvironment environment, IPermissionService permissionService)
        {
            _context = context;
            _environment = environment;
            _permissionService = permissionService;
        }

        public bool IsExsitAcademy(int CityId, string FullName)
        {
            return _context.Academies.Any(a => a.CityId == CityId && a.AcademyFullName == FullName);
        }
        public bool IsExsitMobile(string mobile)
        {
            return _context.Users.IgnoreQueryFilters().Any(u => u.Mobile == mobile);
        }

        public bool IsExsitTeacherMobile(string mobile)
        {
            return _context.NewTeachers.Any(u => u.Mobile == mobile);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public void AddSupportTell(string Tell, int StateId)
        {
            SupportTell supportTell = new SupportTell();
            supportTell.Tell = Tell;
            supportTell.StateId = StateId;
            _context.SupportTells.Add(supportTell);
            _context.SaveChanges();
        }

        public int AddAcademy(Academy academy)
        {
            _context.Academies.Add(academy);
            _context.SaveChanges();
            return academy.AcademyId;
        }

        public int UpdateAcademy(Academy academy)
        {
            _context.Academies.Update(academy);
            _context.SaveChanges();
            return academy.AcademyId;
        }

        public int AddTeacher(NewTeacher newTeacher)
        {
            _context.NewTeachers.Add(newTeacher);
            _context.SaveChanges();
            return newTeacher.TeacherId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string password = PasswordHelper.EncodePasswordMd5(login.Password);
            string mobile = FixedText.FixedEmail(login.Mobile);
            return _context.Users.SingleOrDefault(u => u.Mobile == mobile && u.Password == password);
        }

        public NewTeacher LoginTeacher(LoginTeacherViewModel login)
        {
            string password = PasswordHelper.EncodePasswordMd5(login.Password);
            string mobile = FixedText.FixedEmail(login.Mobile);
            return _context.NewTeachers.SingleOrDefault(u => u.Mobile == mobile && u.Password == password);
        }


        public bool ActiveAccount(string Mobile, string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode && u.Mobile == Mobile);
            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = string.Empty;

            _context.Users.Update(user);
            _context.SaveChanges();
            return true;

        }

        public bool ActiveTeacherAccount(string activeCode)
        {
            var teacher = _context.NewTeachers.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (teacher == null || teacher.IsActive)
                return false;

            teacher.IsActive = true;
            teacher.ActiveCode = string.Empty;

            _context.NewTeachers.Update(teacher);
            _context.SaveChanges();
            return true;

        }

        public bool ForgotActivateCode(string Mobile)
        {
            var user = _context.Users.SingleOrDefault(u => u.Mobile == Mobile);
            if (user.IsActive)
            {
                user.ActiveCode = null;
                Random r = new Random();
                user.ActiveCode = r.Next().ToString().Substring(0, 4);
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            else if (user.ActiveCode != null && user.IsActive)
                return true;
            else
                return false;
        }

        public bool ForgotTeacherActivateCode(string Mobile)
        {
            var teacher = _context.NewTeachers.SingleOrDefault(u => u.Mobile == Mobile);
            if (teacher.IsActive)
            {
                teacher.ActiveCode = null;
                Random r = new Random();
                teacher.ActiveCode = r.Next().ToString().Substring(0, 4);
                _context.NewTeachers.Update(teacher);
                _context.SaveChanges();
                return true;
            }
            else if (teacher.ActiveCode != null && teacher.IsActive)
                return true;
            else
                return false;
        }

        #region Get User
        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public User GetDeleteUserById(int userId)
        {
            return _context.Users.IgnoreQueryFilters().SingleOrDefault(u => u.UserId == userId);
        }

        public NewTeacher GetDeleteTeacherById(int teacherId)
        {
            return _context.NewTeachers.IgnoreQueryFilters().SingleOrDefault(u => u.TeacherId == teacherId);
        }

        public NewTeacher GetTeacherById(int TeacherId)
        {
            return _context.NewTeachers.Find(TeacherId);
        }

        public NewTeacher GetTeacherByTaId(int TeacherId)
        {
            int teacherAcademy = _context.TeacherAcademies.Where(ta => ta.TA_Id == TeacherId).Select(ta => ta.TeacherId).First();
            return _context.NewTeachers.Find(teacherAcademy);
        }
        public int GetUserIdByMobile(string mobile)
        {
            return _context.Users.Single(u => u.Mobile == mobile).UserId;
        }
        public int GetTeacherIdByMobile(string mobile)
        {
            return _context.NewTeachers.Single(u => u.Mobile == mobile).TeacherId;
        }
        public User GetUserByMobile(string mobile)
        {
            return _context.Users.SingleOrDefault(u => u.Mobile == mobile);
        }

        public NewTeacher GetTeacherByMobile(string mobile)
        {
            return _context.NewTeachers.SingleOrDefault(u => u.Mobile == mobile);
        }

        public User GetUserByActiveCode(string Mobile, string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode && u.Mobile == Mobile);
        }

        public NewTeacher GetTeacherByActiveCode(string Mobile, string activeCode)
        {
            return _context.NewTeachers.SingleOrDefault(u => u.ActiveCode == activeCode && u.Mobile == Mobile);
        }

        public Academy GetAcademyById(int AcademyId)
        {
            return _context.Academies.Find(AcademyId);
        }
        #endregion

        #region Wallet
        public int BalanceWalletUser(string mobile)
        {
            int userId = GetUserIdByMobile(mobile);
            //-----------واریز--------------------------------------------//
            var deposit = _context.Wallets.Where(w => w.UserId == userId
                                                      && w.TypeId == 1
                                                      && w.IsPay == true)
                                          .Select(w => w.Amount);

            //---------برداشت ---------------------------------------------//
            var removal = _context.Wallets.Where(w => w.UserId == userId
                                                      && w.TypeId == 2
                                                      && w.IsPay == true)
                                          .Select(w => w.Amount);

            return (deposit.Sum() - removal.Sum());


        }

        public List<WalletInfoViewModel> GetWalletUser(string mobile)
        {
            int userId = GetUserIdByMobile(mobile);

            return _context.Wallets.Where(w => w.UserId == userId && w.IsPay)
                .Select(w => new WalletInfoViewModel()
                {
                    DateTime = w.CreateDate,
                    Amount = w.Amount,
                    Description = w.Description,
                    Type = w.TypeId
                }).ToList();
        }

        public int ChargeWallet(string mobile, int amount, string description, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Today,
                Description = description,
                IsPay = isPay,
                TypeId = 1,
                UserId = GetUserIdByMobile(mobile)
            };

            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

        #endregion

        #region Get Edit Profile view model

        public InformationSupporterViewModel GetSupporterInformation(string mobile)
        {
            var user = GetUserByMobile(mobile);
            Gender Gname = GetGender(user.GenderId);
            Academy Acmy = GetAcademy(user.AcademyId);
            string state = GetStateName(Acmy.CityId);
            InformationSupporterViewModel information = new InformationSupporterViewModel();
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.Province = state;
            information.RegisterDate = user.CreateDate;
            information.UserAvatar = user.UserAvatar;
            information.GenderName = Gname.GenderName;
            return information;
        }
        public InformationUserViewModel GetUserInformation(string mobile)
        {
            var user = GetUserByMobile(mobile);
            Gender Gname = GetGender(user.GenderId);
            Academy Acmy = GetAcademy(user.AcademyId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.Wallet = BalanceWalletUser(mobile);
            information.UserAvatar = user.UserAvatar;
            information.GenderName = Gname.GenderName;
            information.AcademyName = Acmy.AcademyFullName;
            information.AcademyId = Acmy.AcademyId;
            return information;
        }

        public InformationTeacherViewModel GetTeacherInformation(string mobile)
        {
            var user = GetTeacherByMobile(mobile);
            Gender Gname = GetGender(user.GenderId);
            InformationTeacherViewModel information = new InformationTeacherViewModel();
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            //information.Wallet = BalanceWalletUser(mobile);
            information.UserAvatar = user.UserAvatar;
            information.GenderName = Gname.GenderName;
            information.CityName = GetTeacherPlace(user.CityCode);
            return information;
        }

        public InformationTeacherViewModel GetTeacherInformation(int id)
        {
            var user = GetTeacherById(id);
            Gender Gname = GetGender(user.GenderId);
            InformationTeacherViewModel information = new InformationTeacherViewModel();
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.UserAvatar = user.UserAvatar;
            information.GenderName = Gname.GenderName;
            information.CityName = GetTeacherPlace(user.CityCode);
            return information;
        }

        public InformationUserViewModel GetUserInformation(int Id)
        {
            var user = GetUserById(Id);
            Gender Gname = GetGender(user.GenderId);
            Academy Acmy = GetAcademy(user.AcademyId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.Wallet = 0;
            information.UserAvatar = user.UserAvatar;
            information.GenderName = Gname.GenderName;
            information.AcademyName = Acmy.AcademyFullName;
            information.AcademyId = Acmy.AcademyId;
            return information;
        }

        public InformationUserViewModel GetDeleteUserInformation(int Id)
        {
            var user = GetDeleteUserById(Id);
            Gender Gname = GetGender(user.GenderId);
            Academy Acmy = GetAcademy(user.AcademyId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.Wallet = 0;
            information.UserAvatar = user.UserAvatar;
            information.GenderName = Gname.GenderName;
            information.AcademyName = Acmy.AcademyFullName;
            information.AcademyId = Acmy.AcademyId;
            return information;
        }

        public InformationTeacherViewModel GetDeleteTeacherInformation(int Id)
        {
            var teacher = GetDeleteTeacherById(Id);
            Gender Gname = GetGender(teacher.GenderId);
            InformationTeacherViewModel information = new InformationTeacherViewModel();
            information.FirstName = teacher.FirstName;
            information.LastName = teacher.LastName;
            information.Mobile = teacher.Mobile;
            information.RegisterDate = teacher.CreateDate;
            information.Wallet = 0;
            information.UserAvatar = teacher.UserAvatar;
            information.GenderName = Gname.GenderName;
            return information;
        }

        public InformationInspectorViewModel GetInspectorInformation(string mobile)
        {
            var user = GetUserByMobile(mobile);
            int stateId = _context.Academies.Where(a => a.AcademyId == user.AcademyId).Select(a => a.City.StateId).First();
            Gender Gname = GetGender(user.GenderId);
            InformationInspectorViewModel information = new InformationInspectorViewModel();
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            //information.Wallet = BalanceWalletUser(mobile);
            information.UserAvatar = user.UserAvatar;
            information.GenderName = Gname.GenderName;
            information.StateName = GetInspectorPlace(stateId);
            return information;
        }
        public EditProfileViewModel GetDataForEditProfileUser(string mobile)
        {
            return _context.Users.Where(u => u.Mobile == mobile).Select(u => new EditProfileViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarName = u.UserAvatar,
                GenderId = u.GenderId,
                AcademyId = u.AcademyId,
                Mobile = u.Mobile
            }).Single();
        }

        public SurveyViewModel GetDataForSurvay(string mobile)
        {
            return _context.Users.Where(u => u.Mobile == mobile).Select(u => new SurveyViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                GenderId = u.GenderId,
                Mobile = u.Mobile,
                IsPolled = u.IsPolled
            }).Single();
        }

        public EditTeacherProfileViewModel GetDataForEditProfileTeacher(string mobile)
        {
            return _context.NewTeachers.Where(u => u.Mobile == mobile).Select(u => new EditTeacherProfileViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarName = u.UserAvatar,
                GenderId = u.GenderId,
                Mobile = u.Mobile
            }).Single();
        }

        public EditInspectorProfileViewModel GetDataForEditProfileInspector(string mobile)
        {
            return _context.Users.Where(u => u.Mobile == mobile).Select(u => new EditInspectorProfileViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarName = u.UserAvatar,
                GenderId = u.GenderId,
                Mobile = u.Mobile
            }).Single();
        }

        public EditProfileAdminViewModel GetDataForEditAdminProfileUser(string mobile)
        {
            return _context.Users.Where(u => u.Mobile == mobile).Select(u => new EditProfileAdminViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarName = u.UserAvatar,
                GenderId = u.GenderId,
                Mobile = u.Mobile,
                AcademyId = u.AcademyId
            }).Single();
        }

        public EditSupportProfileViewModel GetDataForEditSupportProfileUser(string mobile)
        {
            var user = GetUserByMobile(mobile);
            Academy Acmy = GetAcademy(user.AcademyId);
            return _context.Users.Where(u => u.Mobile == mobile).Select(u => new EditSupportProfileViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarName = u.UserAvatar,
                GenderId = u.GenderId,
                Mobile = u.Mobile,
                Province = GetStateName(Acmy.CityId)
            }).Single();
        }
        #endregion

        #region Edit profile
        public void EditProfile(string mobile, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                profile.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByMobile(mobile);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.UserAvatar = profile.AvatarName;
            user.AcademyId = profile.AcademyId;
            user.GenderId = profile.GenderId;

            UpdateUser(user);
        }

        public void EditTeacherProfile(string mobile, EditTeacherProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                profile.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetTeacherByMobile(mobile);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.UserAvatar = profile.AvatarName;
            user.GenderId = profile.GenderId;

            UpdateTeacher(user);
        }

        public void EditInspectorProfile(string mobile, EditInspectorProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                profile.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByMobile(mobile);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.UserAvatar = profile.AvatarName;
            user.GenderId = profile.GenderId;

            UpdateUser(user);
        }

        public void EditAdminProfile(string mobile, EditProfileAdminViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                profile.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var admin = GetUserByMobile(mobile);
            admin.FirstName = profile.FirstName;
            admin.LastName = profile.LastName;
            admin.UserAvatar = profile.AvatarName;
            admin.GenderId = profile.GenderId;

            UpdateUser(admin);
        }

        public void EditSupportProfile(string mobile, EditSupportProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                profile.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var admin = GetUserByMobile(mobile);
            admin.FirstName = profile.FirstName;
            admin.LastName = profile.LastName;
            admin.UserAvatar = profile.AvatarName;
            admin.GenderId = profile.GenderId;

            UpdateUser(admin);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();

        }

        public void UpdateTeacher(NewTeacher newTeacher)
        {
            _context.Update(newTeacher);
            _context.SaveChanges();

        }
        #endregion

        #region Reterive Gender and Academy

        public List<Gender> Genders()
        {
            var items = _context.Genders.Select(x => new Gender
            {
                GenderId = x.GenderId,
                GenderName = x.GenderName
            }).ToList();

            return items;
        }

        public List<Gender> GetGenders()
        {
            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender { GenderId = 0, GenderName = "بدون مقدار" });
            var items = _context.Genders.Select(x => new Gender
            {
                GenderId = x.GenderId,
                GenderName = x.GenderName
            }).ToList();
            foreach(var Item in items)
            {
                genders.Add(Item);
            }
            return genders;
        }



        public List<Academy> Academies()
        {
            var items = _context.Academies.Select(x => new Academy
            {
                AcademyId = x.AcademyId,
                AcademyName = x.AcademyName,
                AcademyFullName = x.AcademyFullName
            }).ToList();

            return items;
        }

        public List<Employment> Employments()
        {
            var items = _context.Employments.Select(x => new Employment
            {
                EmpId = x.EmpId,
                EmpName = x.EmpName
            }).ToList();

            return items;
        }

        public List<Employment> GetEmployments()
        {
            List<Employment> employments = new List<Employment>();
            employments.Add((new Employment { EmpId = 0, EmpName = "بدون مقدار" }));
            var items = _context.Employments.Select(x => new Employment
            {
                EmpId = x.EmpId,
                EmpName = x.EmpName
            }).ToList();
            foreach(var Item in items)
            {
                employments.Add(Item);
            }
            return employments;
        }

        public List<Education> Educations()
        {
            var items = _context.Educations.Select(x => new Education
            {
                EduId = x.EduId,
                EduName = x.EduName
            }).ToList();

            return items;
        }

        public List<Education> GetEducations()
        {
            List<Education> educations = new List<Education>();
            educations.Add(new Education { EduId = 0, EduName = "بدون مقدار" });
            var items = _context.Educations.Select(x => new Education
            {
                EduId = x.EduId,
                EduName = x.EduName
            }).ToList();
            foreach(var Item in items)
            {
                educations.Add(Item);
            }
            return educations;
        }

        public List<TwoItems> GetTwoItems()
        {
            var items = _context.TwoItems.Select(x => new TwoItems
            {
                ItemId = x.ItemId,
                ItemName = x.ItemName
            }).ToList();

            return items;
        }

        public List<FourItems> GetFourItems()
        {
            var items = _context.FourItems.Select(x => new FourItems
            {
                ItemId = x.ItemId,
                ItemName = x.ItemName
            }).ToList();

            return items;
        }

        public Academy GetAcademy(int Id)
        {
            return _context.Academies.SingleOrDefault(a => a.AcademyId == Id);
        }

        public Gender GetGender(int Id)
        {
            return _context.Genders.SingleOrDefault(a => a.GenderId == Id);
        }

        #endregion

        #region Change Password

        public bool CompareOldPassword(string mobile, string oldPassword)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.Mobile == mobile && u.Password == hashOldPassword);
        }

        public bool CompareTeacherOldPassword(string mobile, string oldPassword)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.NewTeachers.Any(u => u.Mobile == mobile && u.Password == hashOldPassword);
        }

        public void ChangeUserPassword(string mobile, string newPassword)
        {
            var user = GetUserByMobile(mobile);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(user);
        }

        public void ChangeTeacherPassword(string mobile, string newPassword)
        {
            var teacher = GetTeacherByMobile(mobile);
            teacher.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateTeacher(teacher);
        }
        #endregion



        public SideBarAdminPanelViewModel GetSideBarAdminPanelData(string mobile)
        {
            return _context.Users.Where(u => u.Mobile == mobile).Select(u => new SideBarAdminPanelViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarName = u.UserAvatar,
                Gender = u.Gender.GenderName
            }).Single();

        }

        public SideBarAcademyPanelViewModel GetSideBarAcademyPanelData(string mobile)
        {
            SideBarAcademyPanelViewModel sideBar = new SideBarAcademyPanelViewModel();
            var user = _context.Users.Where(u => u.Mobile == mobile).Single();
            int cityId = _context.Academies.Where(a => a.AcademyId == user.AcademyId).Select(a => a.CityId).First();
            int stateId = _context.Cities.Where(c => c.CityId == cityId).Select(c => c.StateId).First();
            List<SupportTell> supports = _context.SupportTells.Where(st => st.StateId == stateId).ToList();
            sideBar.FirstName = user.FirstName;
            sideBar.LastName = user.LastName;
            sideBar.AvatarName = user.UserAvatar;
            sideBar.Gender = _context.Genders.Where(g => g.GenderId == user.GenderId).Select(g => g.GenderName).First();
            sideBar.SupportTells = supports;
            return sideBar;
        }

        #region پر کردن جدول کاربران در پنل ادمین
        public TestUsersForAdminViewModel GetUsers(int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {
            IQueryable<User> result = _context.Users;  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            TestUsersForAdminViewModel list = new TestUsersForAdminViewModel();
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.CurrentPage = pageId;
            list.LastPage = list.PageCount;
            list.PrevPage = Math.Max(pageId - 1, list.CurrentPage);
            list.NextPage = Math.Max(pageId + 1, list.LastPage);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = result.Count();
            return list;
        }

        public TestUsersForAdminViewModel GetUsers(int AcId , int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {
            var RolledUsers = _context.UserRoles.Select(r => r.UserId).ToArray();
            IQueryable<User> result = _context.Users.Where(u => u.AcademyId == AcId && !RolledUsers.Contains(u.UserId));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            TestUsersForAdminViewModel list = new TestUsersForAdminViewModel();
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.CurrentPage = pageId;
            list.LastPage = list.PageCount;
            list.PrevPage = Math.Max(pageId - 1, list.CurrentPage);
            list.NextPage = Math.Max(pageId + 1, list.LastPage);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = _context.Users.Where(u => u.AcademyId == AcId).Count();
            return list;
        }

        public UsersForAdminViewModel GetUsersForSupport(int StateId, int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            IQueryable<User> result = _context.Users.Where(u => cityIds.Contains(u.Academy.CityId));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = result.Count();
            return list;
        }

        public TeachersForAdminViewModel GetTeachers(int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {

            IQueryable<NewTeacher> result = _context.NewTeachers.Where(t => t.IsDelete == false);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            TeachersForAdminViewModel list = new TeachersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.NewTeachers = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.TeacherCounts = result.Count();
            return list;
        }

        public TeachersForAdminViewModel GetProvinceTeachers(int stateId, int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == stateId).Select(c => c.CityId).ToArray();
            IQueryable<NewTeacher> result = _context.NewTeachers.Where(t => t.IsDelete == false && cityIds.Contains(t.CityCode));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            TeachersForAdminViewModel list = new TeachersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.NewTeachers = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            //list.TeacherCounts = _context.NewTeachers.Where(t => t.IsDelete == false && cityIds.Contains(t.CityCode)).Count();
            list.TeacherCounts = result.Count();
            return list;
        }


        public AcademiesForAdminViewModel GetAcademies(int pageId = 1, int take = 10, string filterByName = "", string filterByCity = "")
        {
            IQueryable<Academy> result = _context.Academies.Where(t => t.IsDelete == false);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByName))
            {
                result = result.Where(u => u.AcademyName.Contains(filterByName));
            }

            if (!string.IsNullOrEmpty(filterByCity))
            {
                result = result.Where(u => u.CityId == Int32.Parse(filterByCity));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            AcademiesForAdminViewModel list = new AcademiesForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Academies = result.OrderBy(u => u.AcademyFullName).Skip(skip).Take(takeData).ToList();
            list.AcademyCounts = result.Count();
            return list;
        }

        public AcademiesForAdminViewModel GetProvinceAcademies(int stateId, int pageId = 1, int take = 10, string filterByName = "", string filterByCity = "")
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == stateId).Select(c => c.CityId).ToArray();
            IQueryable<Academy> result = _context.Academies.Where(t => t.IsDelete == false && cityIds.Contains(t.CityId));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByName))
            {
                result = result.Where(u => u.AcademyName.Contains(filterByName));
            }

            if (!string.IsNullOrEmpty(filterByCity))
            {
                result = result.Where(u => u.CityId == Int32.Parse(filterByCity));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            AcademiesForAdminViewModel list = new AcademiesForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Academies = result.OrderBy(u => u.AcademyFullName).Skip(skip).Take(takeData).ToList();
            list.AcademyCounts = result.Count();
            return list;
        }

        public StatesForAdminViewModel GetStatesForAdmin(int pageId = 1, int take = 10, string filterByState = "")
        {
            IQueryable<State> result = _context.States; //lazyLoad;

            if (!string.IsNullOrEmpty(filterByState))
            {
                result = result.Where(s => s.StateName.Contains(filterByState));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            StatesForAdminViewModel list = new StatesForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.States = result.OrderBy(s => s.StateName).Skip(skip).Take(takeData).ToList();
            list.StatesCounts = result.Count();
            return list;
        }

        public CitiesForAdminViewModel GetCitiesForAdmin(int StateId, int pageId = 1, int take = 10, string filterByCity = "")
        {
            IQueryable<City> result = _context.Cities.Where(c => c.StateId == StateId); //lazyLoad;

            if (!string.IsNullOrEmpty(filterByCity))
            {
                result = result.Where(c => c.CityName.Contains(filterByCity));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            CitiesForAdminViewModel list = new CitiesForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Cities = result.OrderBy(c => c.CityName).Skip(skip).Take(takeData).ToList();
            list.CityCounts = result.Count();
            return list;
        }
        #endregion

        #region Academy

        public List<ValidTimesCourse> GetAcademyValidTimes()
        {
            return _context.ValidTimesCourses.ToList();
        }
        public List<AcademyType> GetAcademyTypes()
        {
            return _context.AcademyTypes.ToList();
        }
        public List<string> GetAcademyTypes(int academyId)
        {
            List<string> lst =  _context.AcademyTypeLists.Include(a => a.Academy).Where(a => a.AcademyId == academyId)
                                     .Select(a => a.AcademyType.AcademyTypeTitle).ToList();
            return lst;
        }

        public List<string> GetAcademyValidTimes(int academyId)
        {
            List<string> lst = _context.ValidTimesAcademyLists.Include(a => a.Academy).Where(a => a.AcademyId == academyId)
                                     .Select(a => a.ValidTimesCourse.ValidTimesCourseName).ToList();
            return lst;
        }


        #endregion

        #region اضافه کردن کاربر در پنل ادمین

        public int AddTeacherFromAdmin(CreateTeacherViewModel model)
        {
            NewTeacher teacher = new NewTeacher();
            teacher.GenderId = model.GenderId;
            Random r = new Random();
            teacher.ActiveCode = r.Next().ToString().Substring(0, 4);
            teacher.CreateDate = DateTime.Now;
            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.IsActive = model.IsActive;
            teacher.Mobile = model.Mobile;
            teacher.CityCode = model.CityCode;
            teacher.Password = PasswordHelper.EncodePasswordMd5(model.Password);

            #region Save Avatar

            if (model.UserAvatar != null)
            {
                string imagePath = "";

                //-------Upload New User Image --------//
                teacher.UserAvatar = GeneratorName.GenrateUniqeCode() + Path.GetExtension(model.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", teacher.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UserAvatar.CopyTo(stream);
                }
            }
            else
            {
                teacher.UserAvatar = "default.png";
            }

            #endregion

            return AddTeacher(teacher);
        }

        public int AddUserFromAdmin(CreateUserViewModel model)
        {
            User user = new User();
            user.GenderId = model.GenderId;
            user.AcademyId = model.AcademyId;
            Random r = new Random();
            user.ActiveCode = r.Next().ToString().Substring(0, 4);
            user.CreateDate = DateTime.Now;
            user.FirstName = FixedText.FixedTxt(model.FirstName);
            user.LastName = FixedText.FixedTxt(model.LastName);
            user.IsActive = model.IsActive;
            user.Mobile = FixedText.FixedMobile(model.Mobile);
            user.Password = PasswordHelper.EncodePasswordMd5(FixedText.FixedTxt(model.Password));

            #region Save Avatar

            if (model.UserAvatar != null)
            {
                string imagePath = "";

                //-------Upload New User Image --------//
                user.UserAvatar = GeneratorName.GenrateUniqeCode() + Path.GetExtension(model.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UserAvatar.CopyTo(stream);
                }
            }
            else
            {
                user.UserAvatar = "default.png";
            }

            #endregion

            return AddUser(user);
        }

        public int AddUserFromSupport(CreateUserViewModel model)
        {
            User user = new User();
            user.GenderId = model.GenderId;
            user.AcademyId = model.AcademyId;
            user.CreateDate = DateTime.Now;
            user.FirstName = FixedText.FixedTxt(model.FirstName);
            user.LastName = FixedText.FixedTxt(model.LastName);
            user.IsActive = model.IsActive;
            user.Mobile = FixedText.FixedMobile(model.Mobile);
            user.Password = PasswordHelper.EncodePasswordMd5(FixedText.FixedTxt(model.Password));

            #region Save Avatar

            if (model.UserAvatar != null)
            {
                string imagePath = "";

                //-------Upload New User Image --------//
                user.UserAvatar = GeneratorName.GenrateUniqeCode() + Path.GetExtension(model.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UserAvatar.CopyTo(stream);
                }
            }
            else
            {
                user.UserAvatar = "default.png";
            }

            #endregion

            return AddUser(user);
        }

        public int AddAcademyFromAdmin(CreateAcademyViewModel model)
        {
            Academy academy = new Academy();
            academy.AcademyName = model.AcademyName;
            academy.CityId = model.CityId;
            academy.PostalCode = model.PostalCode;
            academy.AcademyTell = model.AcademyTell;
            academy.OwnerName = model.OwnerName;
            academy.OwnerId = model.OwnerId;
            academy.OwnerTell = model.OwnerTell;
            academy.ManagerName = model.ManagerName;
            academy.ManagerId = model.ManagerId;
            academy.ManagerTell = model.ManagerTell;
            academy.AcademyAddress = model.AcademyAddress;
            academy.AcademyFullName = model.AcademyFullName;
            academy.CreateDate = model.CreateDate;
            academy.IsDelete = false;
            academy.BBBServersId = model.BBBServerId;
            return AddAcademy(academy);
        }

        public int EditAcademyFromAdmin(EditAcademyViewModel model , int AcademyId)
        {
            var academy = GetAcademyById(AcademyId);
            academy.AcademyName = model.AcademyName;
            academy.CityId = model.CityCode;
            academy.PostalCode = model.PostalCode;
            academy.AcademyTell = model.AcademyTell;
            academy.OwnerName = model.OwnerName;
            academy.OwnerId = model.OwnerId;
            academy.OwnerTell = model.OwnerTell;
            academy.ManagerName = model.ManagerName;
            academy.ManagerId = model.ManagerId;
            academy.ManagerTell = model.ManagerTell;
            academy.AcademyAddress = model.AcademyAddress;
            academy.AcademyFullName = model.AcademyFullName;
            academy.CreateDate = model.CreateDate;
            academy.IsDelete = false;
            academy.BBBServersId = model.BBBServerId;
            return UpdateAcademy(academy);
        }

        public void EditTypesforAcademy(List<int> TypesIds, int AcademyId)
        {
            _context.AcademyTypeLists.Where(at => at.AcademyId == AcademyId).ToList().ForEach(at => _context.AcademyTypeLists.Remove(at));
            AddTypesToAcademy(TypesIds, AcademyId);
        }

        public void EditValidTimesForAcademy(List<int> ValidTimesIds, int AcademyId)
        {
            _context.ValidTimesAcademyLists.Where(vt => vt.AcademyId == AcademyId).ToList().ForEach(vt => _context.ValidTimesAcademyLists.Remove(vt));
            AddValidTimesToAcademy(ValidTimesIds, AcademyId);
        }

        public void UpdateCourseServer(int AcademyId, int ServerId)
        {
            var Courses = _context.Courses.Where(c => c.CompanyId == AcademyId).ToList();
            if(Courses.Count != 0)
            {
                foreach (Course c in Courses)
                {
                    c.ServerId = ServerId;
                    _context.Courses.Update(c);
                    _context.SaveChanges();
                }
            }
        }

        public void AddTypesToAcademy(List<int> TypesIds, int AcademyId)
        {
            foreach (int TypeId in TypesIds)
            {
                _context.AcademyTypeLists.Add(new AcademyTypeList()
                {
                    AcademyTypeId = TypeId,
                    AcademyId = AcademyId
                });

            }
            _context.SaveChanges();
        }

       

        public void AddValidTimesToAcademy(List<int> ValidTimesIds, int AcademyId)
        {
            foreach (int ValidTimesId in ValidTimesIds)
            {
                _context.ValidTimesAcademyLists.Add(new ValidTimesAcademyList()
                {
                    ValidTimesCourseId = ValidTimesId,
                    AcademyId = AcademyId
                });

            }

            _context.SaveChanges();
        }


        #endregion

        #region دریافت اطلاعات کاربر جهت ویرایش در پنل ادمین
        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId).Select(u => new EditUserViewModel()
            {
                GenderId = u.GenderId,
                AcademyName = u.Academy.AcademyFullName,
                AcademyId = u.AcademyId,
                AvatarName = u.UserAvatar,
                FirstName = u.FirstName,
                IsActive = u.IsActive,
                LastName = u.LastName,
                Mobile = u.Mobile,
                UserId = u.UserId,
                UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),

            }).Single();
        }

        public EditTeacherViewModel GetTeacherForShowInEditMode(int teacherId)
        {
            return _context.NewTeachers.Where(t => t.TeacherId == teacherId).Select(t => new EditTeacherViewModel()
            {
                GenderId = t.GenderId,
                CityCode = t.CityCode,
                AvatarName = t.UserAvatar,
                FirstName = t.FirstName,
                IsActive = t.IsActive,
                LastName = t.LastName,
                Mobile = t.Mobile,
                TeacherId = t.TeacherId,
                TeacherGroups = t.TeacherGroup.Select(g => g.GroupId).ToList(),
            }).Single();
        }

        public EditAcademyViewModel GetAcademyForEdit(int AcademyId)
        {
            return _context.Academies.Where(a => a.AcademyId == AcademyId).Select(a => new EditAcademyViewModel()
            {
                AcademyName = a.AcademyName,
                AcademyFullName = a.AcademyFullName,
                ManagerId = a.ManagerId,
                ManagerName = a.ManagerName,
                ManagerTell = a.ManagerTell,
                CityName = a.City.CityName,
                OwnerId = a.OwnerId,
                OwnerName = a.OwnerName,
                OwnerTell = a.OwnerTell,
                CreateDate = a.CreateDate,
                CityCode = a.CityId,
                AcademyAddress = a.AcademyAddress,
                AcademyTell = a.AcademyTell,
                PostalCode = a.PostalCode,
                BBBServerId = a.BBBServersId,
                AcademyType = a.AcademyTypeLists.Select(at => at.AcademyTypeId).ToList(),
                ValidTimesCourse = a.ValidTimesAcademyLists.Select(vta => vta.ValidTimesCourseId).ToList()
            }).Single();
        }
        #endregion

        #region ویرایش کاربر توسط مدیر
        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            var user = GetUserById(editUser.UserId);
            user.Mobile = FixedText.FixedMobile(editUser.Mobile);
            user.FirstName = FixedText.FixedTxt(editUser.FirstName);
            user.LastName = FixedText.FixedTxt(editUser.LastName);
            user.AcademyId = editUser.AcademyId;
            user.IsActive = editUser.IsActive;
            user.GenderId = editUser.GenderId;
            user.ActiveCode = null;

            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(FixedText.FixedTxt(editUser.Password));
            }

            if (editUser.UserAvatar != null)
            {
                string imagePath = "";
                if (editUser.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", editUser.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                editUser.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", editUser.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }

            user.UserAvatar = editUser.AvatarName;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void EditTeacherFromAdmin(EditTeacherViewModel editTeacher)
        {
            var teacher = GetTeacherById(editTeacher.TeacherId);
            teacher.GenderId = editTeacher.GenderId;
            teacher.FirstName = editTeacher.FirstName;
            teacher.LastName = editTeacher.LastName;
            teacher.Mobile = editTeacher.Mobile;
            teacher.IsActive = editTeacher.IsActive;
            teacher.CityCode = editTeacher.CityCode;

            if (!string.IsNullOrEmpty(editTeacher.Password))
            {
                teacher.Password = PasswordHelper.EncodePasswordMd5(editTeacher.Password);
            }

            if (editTeacher.UserAvatar != null)
            {
                string imagePath = "";
                if (editTeacher.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", editTeacher.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                editTeacher.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(editTeacher.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", editTeacher.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editTeacher.UserAvatar.CopyTo(stream);
                }
            }

            teacher.UserAvatar = editTeacher.AvatarName;
            _context.NewTeachers.Update(teacher);
            _context.SaveChanges();


        }
        
        #endregion

        #region استخراج لیست کاربران حذف شده
        public UsersForAdminViewModel GetDeleteUsersForSupport(int StateId, int pageId = 1, int take = 10, string filterByLastName = "",
            string filterByMobile = "")
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete && cityIds.Contains(u.Academy.CityId));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = result.Count();

            return list;
        }

        public UsersForAdminViewModel GetDeleteUsers(int AcId, int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete && u.AcademyId == AcId);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = result.Count();

            return list;
        }

        public UsersForAdminViewModel GetDeleteUsers(int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = result.Count();

            return list;
        }

        public void RestoreUsers(int userId)
        {
            var user = GetDeleteUserById(userId);
            user.IsDelete = false;
            user.IsActive = true;
            UpdateUser(user);
        }

        public void RestoreTeachers(int teacherId)
        {
            var teacher = GetDeleteTeacherById(teacherId);
            teacher.IsDelete = false;
            teacher.IsActive = true;
            UpdateTeacher(teacher);
        }

        public TeachersForAdminViewModel GetDeleteTeacher(int pageId = 1, int take = 10, string filterByLastName = "",
            string filterByMobile = "")
        {
            IQueryable<NewTeacher> result = _context.NewTeachers.IgnoreQueryFilters().Where(u => u.IsDelete);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            TeachersForAdminViewModel list = new TeachersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.NewTeachers = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.TeacherCounts = result.Count();

            return list;
        }

        public TeachersForAdminViewModel GetProvinceDeleteTeacher(int stateId, int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "")
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == stateId).Select(c => c.CityId).ToArray();
            IQueryable<NewTeacher> result = _context.NewTeachers.IgnoreQueryFilters().Where(t => t.IsDelete && cityIds.Contains(t.CityCode));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByLastName))
            {
                result = result.Where(u => u.LastName.Contains(filterByLastName) || u.FirstName.Contains(filterByLastName));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile.Contains(filterByMobile));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            TeachersForAdminViewModel list = new TeachersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.NewTeachers = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.TeacherCounts = _context.NewTeachers.IgnoreQueryFilters().Where(t => t.IsDelete && cityIds.Contains(t.CityCode)).Count();

            return list;
        }
        #endregion

        #region حذف کاربر
        public void DeleteUser(int userId)
        {
            var user = GetUserById(userId);
            user.IsDelete = true;
            user.IsActive = false;
            UpdateUser(user);
            _permissionService.RemoveRolesUser(user.UserId);
        }

        public void DeleteTeacher(int teacherId)
        {
            var teacher = GetTeacherById(teacherId);
            teacher.IsDelete = true;
            teacher.IsActive = false;
            UpdateTeacher(teacher);
        }

        public List<TeachersForSurvay> GetTeachers(int[] CourseId)
        {
            List<TeachersForSurvay> teachers = new List<TeachersForSurvay>();
            List<Course> courses = new List<Course>();
            courses = _context.Courses.Where(c => CourseId.Contains(c.CourseId)).ToList();
            foreach(Course c in courses)
            {
                IQueryable<TeachersForSurvay> t = _context.NewTeachers.Where(Teach => Teach.TeacherId == c.TeacherId).Select(Teach => new TeachersForSurvay
                {
                    TeacherId = Teach.TeacherId,
                    TeacherFullName = Teach.Gender.GenderName + " " + Teach.FirstName + " " + Teach.LastName,
                    TeacherType = c.GroupId,
                    TeacherTypeName = _context.CourseGroups.Where(CG => CG.GroupId == c.GroupId).Select(CG => CG.GroupTitle).First()
                });
                if (!teachers.Any(x => x.TeacherId == t.Single().TeacherId))
                    teachers.Add(t.Single());
            }

            return teachers;
        }

        public List<SelectListItem> GetState()
        {
            return _context.States.Select(s => new SelectListItem()
            {
                Value = s.StateId.ToString(),
                Text = s.StateName
            }).ToList();
        }

        public List<SupportTell> GetStatesTells(int StateId)
        {
            return _context.SupportTells.Where(st => st.StateId == StateId).ToList();
        }

        public string GetStateNameById(int stateId)
        {
            return _context.States.Where(s => s.StateId == stateId).Select(s => s.StateName).First();
        }

        public List<SelectListItem> GetCity(int stateId)
        {
            return _context.Cities.Where(c => c.StateId == stateId)
                .Select(c => new SelectListItem()
                {
                    Text = c.CityName,
                    Value = c.CityId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetAcademies(int cityId)
        {
            return _context.Academies.Where(a => a.CityId == cityId) 
                .Select(a => new SelectListItem()
            {
                Value = a.AcademyId.ToString(),
                Text = a.AcademyName
            }).ToList();
        }

        public List<SelectListItem> GetAcademyName(int AcademyId)
        {
            return _context.Academies.Where(a => a.AcademyId == AcademyId)
                .Select(a => new SelectListItem()
                {
                    Value = a.AcademyId.ToString(),
                    Text = a.AcademyFullName
                }).ToList();
        }

        public string GetAcademyFullName(int AcademyId)
        {
            return _context.Academies.Where(a => a.AcademyId == AcademyId)
                .Select(a => a.AcademyFullName).First();
        }

        public int GetCityForAcademies(int academyId)
        {
            int b = _context.Academies.Where(a => a.AcademyId == academyId).Select(a => a.CityId).First();
            return b;
        }

        public int GetMaxUsersForAcademies(int academyId)
        {
            return _context.Cities.Where(c => c.CityId == GetCityForAcademies(academyId)).Select(c => c.MaxUsers).First();
        }

        public int GetStateFoAcademy(int cityId)
        {
            int a = _context.Cities.Where(c => c.CityId == cityId).Select(c => c.StateId).First();
            return a;
        }

        public string GetStateName(int cityId)
        {
            int a = _context.Cities.Where(c => c.CityId == cityId).Select(c => c.StateId).First();
            string b = _context.States.Where(s => s.StateId == a).Select(s => s.StateName).First();
            return b;
        }

        public string GetTeacherPlace(int cityId)
        {
           return _context.States.Join(_context.Cities,
                                           state => state.StateId,
                                           city => city.State.StateId,
                                           (state, city) => new
                                           {
                                               txtCity = city.CityName,
                                               txtState = state.StateName,
                                               label = state.StateName + " / " + city.CityName,
                                               val = city.CityId
                                           }).Where(c => c.val == cityId).Select(c => c.label).First();
        }

        public string GetInspectorPlace(int stateId)
        {
            return _context.States.Join(_context.Cities,
                                            state => state.StateId,
                                            city => city.State.StateId,
                                            (state, city) => new
                                            {
                                                txtCity = city.CityName,
                                                txtState = state.StateName,
                                                label = state.StateName,
                                                val = state.StateId
                                            }).Where(s => s.val == stateId).Select(s => s.label).First();
        }

        public string GetAcademyPlace(int academyId)
        {
            return _context.Academies.Where(a => a.AcademyId == academyId).Select(a => a.AcademyFullName).First();
        }

        public List<CourseGroup> GetTeacherGroups()
        {
            return _context.CourseGroups.ToList();
        }
        public List<BBBServers> GetServers()
        {
            return _context.BBBServers.ToList();
        }

        public BBBServerEditViewModel GetServerById(int id)
        {
            return _context.BBBServers.Where(b => b.Id == id).Select(b => new BBBServerEditViewModel()
            {
                Id = b.Id,
                ServerName = b.ServerName,
                ServerSharesSecret = b.ServerSharesSecret,
                ServerUrl = b.ServerUrl,
                IsActive = b.IsActive
            }).Single();
        }

        public void UpdateBBBServer(BBBServerEditViewModel model , int id)
        {
            var server = _context.BBBServers.SingleOrDefault(s => s.Id == id);
            server.ServerName = model.ServerName;
            server.ServerSharesSecret = model.ServerSharesSecret;
            server.ServerUrl = model.ServerUrl;
            server.IsActive = model.IsActive;
            _context.BBBServers.Update(server);
            _context.SaveChanges();
        }

        public int AddServer(BBBServers bBBServers)
        {
            _context.BBBServers.Add(bBBServers);
            _context.SaveChanges();
            return bBBServers.Id;
        }

        public int AddCity(CreateCityViewModel createCityView)
        {
            if (_context.Cities.Any(c => createCityView.CityName.Equals(c.CityName)))
                return 0;
            City city = new City();
            city.CityName = createCityView.CityName;
            city.MaxUsers = createCityView.MaxUsers;
            city.StateId = createCityView.StateId;
            _context.Cities.Add(city);
            _context.SaveChanges();
            return city.CityId;
        }

        public CityEditViewModel GetCityForEdit(int CityId)
        {
            var city = _context.Cities.Find(CityId);
            CityEditViewModel cityEditViewModel = new CityEditViewModel();
            cityEditViewModel.CityId = city.CityId;
            cityEditViewModel.CityName = city.CityName;
            cityEditViewModel.MaxUsers = city.MaxUsers;
            cityEditViewModel.StateId = city.StateId;
            return cityEditViewModel;
        }

        public int EditCity(CityEditViewModel cityEdit, int CityId)
        {
            //if (_context.Cities.Any(c => cityEdit.CityName.Equals(c.CityName) && c.StateId == cityEdit.StateId))
            //    return 0;
            City city = _context.Cities.Find(CityId);
            city.CityName = cityEdit.CityName;
            city.MaxUsers = cityEdit.MaxUsers;
            _context.Cities.Update(city);
            _context.SaveChanges();
            return city.CityId;
        }

        public void AddEvent(int TeacherId, int EventId, DateTime CreateDate, string TeacherIP, string Extra , int CourseId, int ServerId, string ClassId)
        {
            Event evnt = new Event();
            evnt.TeacherId = TeacherId;
            evnt.TeacherIP = TeacherIP;
            evnt.EventId = EventId;
            evnt.CreateDate = CreateDate;
            evnt.Extra = Extra;
            evnt.CourseId = CourseId;
            evnt.ServerId = ServerId;
            evnt.ClassId = ClassId;
            _context.Events.Add(evnt);
            _context.SaveChanges();
        }

        public void AddUserEvent(int UserId, int EventId, DateTime CreateDate, string UserIP, string Extra , int CourseId)
        {
            UserEvent evnt = new UserEvent();
            evnt.UserId = UserId;
            evnt.UserIP = UserIP;
            evnt.EventId = EventId;
            evnt.CreateDate = CreateDate;
            evnt.Extra = Extra;
            evnt.CourseId = CourseId;
            _context.UserEvents.Add(evnt);
            _context.SaveChanges();
        }

        public void AddGeneralEvent(int UserId, int EventId, DateTime CreateDate, string UserIP, string Extra)
        {
            GeneralEvents generalEvent = new GeneralEvents();
            generalEvent.UserId = UserId;
            generalEvent.EventId = EventId;
            generalEvent.CreateDate = CreateDate;
            generalEvent.UserIP = UserIP;
            generalEvent.Extra = Extra;
            _context.GeneralEvents.Add(generalEvent);
            _context.SaveChanges();
        }

        #endregion
    }
}
