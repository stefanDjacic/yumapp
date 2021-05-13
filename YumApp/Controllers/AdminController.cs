using EntityLibrary;
using EntityLibrary.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Controllers.HelperAndExtensionMethods;
using YumApp.Models;

namespace YumApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ICRUDRepository<Post> _postRepository;
        private readonly ICRDRepository<Yummy_Post> _yummy_PostRepository;
        private readonly ICRDRepository<Ingredient> _ingredientRepository;
        private readonly AppUserManager _appUserManager;
        private readonly string _ingredientPhotoFolderPath;

        public AdminController(ICRUDRepository<Post> postRepository,
                               ICRDRepository<Yummy_Post> yummy_PostRepository,
                               ICRDRepository<Ingredient> ingredientRepository,
                               AppUserManager appUserManager,
                               IWebHostEnvironment webHostEnvironment)
        {
            _postRepository = postRepository;
            _yummy_PostRepository = yummy_PostRepository;
            _ingredientRepository = ingredientRepository;
            _appUserManager = appUserManager;
            _ingredientPhotoFolderPath = webHostEnvironment.ContentRootPath + @"\wwwroot\Photos\IngredientPhotos\";
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAPost(int id)
        {
            //Gets reported post
            Post reportedPost = await _postRepository.GetSingle(id);

            //Gets list of yummy_Post instances with users that have liked it
            List<Yummy_Post> samePostLikedByOthers = await _yummy_PostRepository.GetAll()
                                                                                .Where(yp => yp.PostId == id)
                                                                                .ToListAsync();
            //Removes the instances from intermediary table (not very efficient, because of constantly SaveChanges(), RemoveRange() would be much better)
            foreach (var post in samePostLikedByOthers)
            {
                await _yummy_PostRepository.Remove(post);
            }

            //Removes the reported post
            await _postRepository.Remove(reportedPost);

            return Json(new { redirectToUrl = Url.Action("Profile", "User", new { id = 1 }) });
        }

        [HttpGet]
        public IActionResult CreateIngredient()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient(IngredientModel model)
        {
            try
            {
                //Checks if model is valid
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //Changes photo path from the model
                model.PhotoPath = ControllerHelperMethods.UpdateIngridientPhotoPath(_ingredientPhotoFolderPath, model.Photo.FileName);

                //Converts model to Ingredient type
                Ingredient ingredientEntity = model.ToIngredientEntity();

                //Adds new ingredient to database
                Ingredient newIngredient = await _ingredientRepository.Add(ingredientEntity);

                //Adds photo to server
                await ControllerHelperMethods.SaveIngredientPhoto(model.Photo, _ingredientPhotoFolderPath);

                return RedirectToAction("IngredientInfo", new { id = newIngredient.Id });
            }
            catch
            {

                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> IngredientInfo(int id)
        {
            //Gets specified ingredient
            IngredientModel ingredient = _ingredientRepository.GetAll()
                                                              .ToIngredientModel()
                                                              .SingleOrDefault(i => i.Id == id);
            //Need id for button attribute
            ViewBag.CurrentUserId = await _appUserManager.GetCurrentUserIdAsync(User);

            return View(ingredient);
        }
    }
}
