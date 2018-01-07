using FlyModel.Model.Data;
using FlyModel.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class PhotoManager
    {
        public static PhotoManager Instance;

        public static PhotoManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new PhotoManager();
            }
            return Instance;
        }

        public Dictionary<string, int> AllCatPictureOwners = new Dictionary<string, int>();
        public List<PhotoData> AllCatPhotoDatas = new List<PhotoData>();

        public List<PhotoData> AllScenePhotoDatas = new List<PhotoData>();

        public void InitAllPictureOwners()
        {
            var dir = System.IO.Path.GetDirectoryName(ResourceLoader.CatPictureCacheRoot);
            if (!System.IO.Directory.Exists(dir))
            {

            }
            else
            {
                string[] files = Directory.GetFiles(dir, "*.jpg");

                string fullPath = "";
                string photoName = "";
                string[] path = null;
                foreach (var photo in files)
                {
                    fullPath = photo.Replace('\\', '/');
                    path = fullPath.Split('/');
                    photoName = path[path.Length - 1];

                    AddOneCatPhoto(photoName);
                }
            }

            InitAllScenePhototDatas();
        }

        public void InitAllScenePhototDatas()
        {
            var dir = System.IO.Path.GetDirectoryName(ResourceLoader.ScenePictureCacheRoot);
            if (!System.IO.Directory.Exists(dir))
            {

            }
            else
            {
                string[] files = Directory.GetFiles(dir, "*.jpg");

                string fullPath = "";
                string photoName = "";
                string[] path = null;
                foreach (var photo in files)
                {
                    fullPath = photo.Replace('\\', '/');
                    path = fullPath.Split('/');
                    photoName = path[path.Length - 1];

                    AddOneScenePhoto(photoName);
                }
            }
        }

        public void AddOneCatPhoto(string photoName)
        {
            var nameAry = photoName.Split('-');
            var tempPhotoOwner = nameAry[0];

            var temp = new PhotoData();
            temp.Owner = tempPhotoOwner;
            temp.PhotoName = photoName;
            AllCatPhotoDatas.Add(temp);

            if (AllCatPictureOwners.ContainsKey(tempPhotoOwner) == false)
            {
                AllCatPictureOwners.Add(tempPhotoOwner, 1);
            }
            else
            {
                AllCatPictureOwners[tempPhotoOwner] = AllCatPictureOwners[tempPhotoOwner] + 1;
            }
        }

        public void AddOneScenePhoto(string photoName)
        {
            var nameAry = photoName.Split('-');
            var tempPhotoOwner = nameAry[0];
            var temp = new PhotoData();
            temp.Mode = UI.EnumConfig.PictureMode.Screen;
            temp.PhotoName = photoName;
            
            AllScenePhotoDatas.Add(temp);
        }

        public void DeleteOneCatPhoto(string photoName)
        {
            var nameAry = photoName.Split('-');
            var tempPhotoOwner = nameAry[0];

            AllCatPictureOwners[tempPhotoOwner] = AllCatPictureOwners[tempPhotoOwner] - 1;
            if (AllCatPictureOwners[tempPhotoOwner] == 0)
            {
                AllCatPictureOwners.Remove(tempPhotoOwner);
            }

            foreach (var catPhoto in AllCatPhotoDatas)
            {
                if (catPhoto.PhotoName == photoName)
                {
                    AllCatPhotoDatas.Remove(catPhoto);
                    break;
                }
            }

            var cachePath = System.IO.Path.Combine(ResourceLoader.CatPictureCacheRoot, photoName);
            if (System.IO.File.Exists(cachePath))
            {
                System.IO.File.Delete(cachePath);
            }
        }
        public void DeleteOneScenePhoto(string photoName)
        {
            var nameAry = photoName.Split('-');
            var tempPhotoOwner = nameAry[0];
           
            foreach (var scenePhoto in AllScenePhotoDatas)
            {
                if (scenePhoto.PhotoName == photoName)
                {
                    AllScenePhotoDatas.Remove(scenePhoto);
                    break;
                }
            }

            var cachePath = System.IO.Path.Combine(ResourceLoader.ScenePictureCacheRoot, photoName);
            if (System.IO.File.Exists(cachePath))
            {
                System.IO.File.Delete(cachePath);
            }
        }


        public int GetPhotoMaxCount()
        {
            return Model.Data.EffectiveData.PHOTO_NUMS_ADDITION + NumeralConfig.MAX_PHOTOS;
        }
    }
}
