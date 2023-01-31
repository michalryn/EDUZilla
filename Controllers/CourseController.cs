﻿using EDUZilla.Models;
using EDUZilla.Services;
using EDUZilla.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDUZilla.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var course = await _courseService.GetCourseViewModelAsync(id);

            if(course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _courseService.GetEditCourseViewModel(id);

            //error handle
            if(course == null)
            {
                return View();
            }
            course.AvailableTeachers?.Insert(0, new SelectListItem() { Value = "", Selected = true, Disabled = true });
            course.AvailableClasses?.Insert(0, new SelectListItem() { Value = "", Selected = true, Disabled = true });

            //course.AvailableTeachers?.Insert(0, new SelectListItem() { Value = "", Text = "Wybierz nauczyciela", Selected = true, Disabled = true });
            //course.AvailableClasses?.Insert(0, new SelectListItem() { Value = "", Text = "Wybierz klasę", Selected = true, Disabled = true });

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeName(int id, string newName)
        {
            var check = await _courseService.CheckIfCourseExists(newName);
            
            if(check)
            {
                return RedirectToAction("EditCourse", new { id = id });
            }

            var result = await _courseService.ChangeCourseName(id, newName);

            if(!result)
            {
                return RedirectToAction("EditCourse", new { id = id });
            }

            return RedirectToAction("ViewCourses");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTeacher(int id, string? teacherId)
        {
            if(teacherId == null)
            {
                return RedirectToAction("ViewCourses");
            }

            var result = await _courseService.AddTeacherAsync(id, teacherId);

            //handle error
            if(!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(int id, string teacherId)
        {
            var result = await _courseService.DeleteTeacherAsync(id, teacherId);
            
            //handle error
            if(!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddClass(int id, int? classId)
        {
            if(classId == null)
            {
                return RedirectToAction("ViewCourses");
            }

            var result = await _courseService.AddClassAsync(id, (int)classId);

            if (!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteClass(int id, int? classId)
        {
            if (classId == null)
            {
                return RedirectToAction("ViewCourses");
            }

            var result = await _courseService.DeleteClassAsync(id, (int)classId);

            if (!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("EditCourse", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseByIdAsync(id);

            if(!result)
            {
                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("ViewCourses");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCourse(string name)
        {
            if(ModelState.IsValid)
            {
                var check = await _courseService.CheckIfCourseExists(name);

                if(check)
                {
                    return RedirectToAction("ViewCourses");
                }

                var result = await _courseService.AddCourseAsync(name);

                //error handle
                if(!result)
                {
                    return BadRequest();
                }

                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("ViewCourses");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewCourses()
        {
            List<CourseListViewModel> list = await _courseService.GetCoursesListAsync();

            return View(list);
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> ViewClasses(int id)
        {
            var classes = await _courseService.GetClassListViewModelAsync(id);
            var course = await _courseService.GetCourseViewModelAsync(id);

            if(classes == null || course == null)
            {
                return NotFound();
            }

            return View(new { classes = classes, courseId = id, courseName = course.CourseName });
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult AddFile(int id)
        {
            return View(id);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> AddFile(AddFileForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            
            if(form.File.ContentType != "application/pdf")
            {
                return View(form);
            }

            var result = await _courseService.AddFile(form);

            if(!result)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", new {id = form.CourseId});
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> DeleteFile(int id, int courseId)
        {
            var result = await _courseService.DeleteFile(id);

            if(!result)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", new { id = courseId });
        }

        public async Task<FileResult> DownloadFile(int id)
        {
            var file = await _courseService.GetFile(id);

            if (file == null)
            {
                return null;
            }

            return File(file.Data, "application/pdf", file.FileName);
        }
    }
}
