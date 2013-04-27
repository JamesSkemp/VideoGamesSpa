using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using JamesRSkemp.VideoGamesSpa.Web.Models;

namespace JamesRSkemp.VideoGamesSpa.Web.Controllers
{
    public class VideoGamesController : ApiController
    {
        //
        // GET: /VideoGames/

        public VideoGame[] Get()
        {
			return VideoGame.GetGames();
        }

    }
}
