using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HobbyLobbySB.Models;
using HobbyLobbySB.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HobbyLobbySB.Controllers
{
    [Route("api/[controller]")]
    public class CoreController : Controller
    {
        // GET: api/Professors
        [HttpPost]
        [Route("v1/BestWayToFit")]
        public async Task<IActionResult> BestWayToFit([FromBody] BoxLocation boxLocation )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BoxWay bestFit = FindTheBestWay(boxLocation.Location, boxLocation.Box);
            return Ok(bestFit.ToString());
        }

        [HttpPost]
        [Route("v1/ShowBestWayCalcutions")]
        public async Task<IActionResult> ShowBestWayCalcutions([FromBody] BoxLocation boxLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BoxWay bestFit = FindTheBestWay(boxLocation.Location, boxLocation.Box);
            
            return Ok(ShowBestWayCalcutions(boxLocation.Location, boxLocation.Box, bestFit));
        }

        [HttpPost]
        [Route("v1/HowManyCanFit")]
        public async Task<IActionResult> HowManyCanFit([FromBody] BoxLocation boxLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BoxWay bestFit = FindTheBestWay(boxLocation.Location, boxLocation.Box);

            return Ok(HowManyCanFit(boxLocation.Location, boxLocation.Box, bestFit));
        }

        [HttpPost]
        [Route("v1/InchesLeft")]
        public async Task<IActionResult> InchesLeft([FromBody] BoxLocation boxLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BoxWay bestFit = FindTheBestWay(boxLocation.Location, boxLocation.Box);

            return Ok(InchesLeft(boxLocation.Location, boxLocation.Box, bestFit));
        }

        [HttpPost]
        [Route("v1/CompleteReport")]
        public async Task<IActionResult> CompleteReport([FromBody] BoxLocation boxLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BoxWay bestFit = FindTheBestWay(boxLocation.Location, boxLocation.Box);

            string temp = "";
            temp += "Location: LWH\n";
            temp += "Box     : " + bestFit + "\n\n";
            temp += ShowBestWayCalcutions(boxLocation.Location, boxLocation.Box, bestFit)+"\n";
            temp += "Number total of boxes: " + HowManyCanFit(boxLocation.Location, boxLocation.Box, bestFit) + "\n\n";
            Location locationLeft = InchesLeft(boxLocation.Location, boxLocation.Box, bestFit);
            temp += "Inches Left\n";
            temp += "L: " + locationLeft.Length+"\n";
            temp += "W: " + locationLeft.Width + "\n";
            temp += "H: " + locationLeft.Height + "\n";
            return Ok(temp);
        }


        private BoxWay FindTheBestWay(Location location,Box box)
        {
            int[] lengthDivision = new int[3];
            int[] widthDivision = new int[3];
            int[] heightDivision = new int[3];
            int[] cases = new int[6];

            //Dividing the Length of the location by all the sides of the box
            lengthDivision[0] = (int)(location.Length / box.Length);
            lengthDivision[1] = (int)(location.Length / box.Width);
            lengthDivision[2] = (int)(location.Length / box.Height);
            //Dividing the Width of the location by all the sides of the box
            widthDivision[0] = (int)(location.Width / box.Length);
            widthDivision[1] = (int)(location.Width / box.Width);
            widthDivision[2] = (int)(location.Width / box.Height);
            //Dividing the Height of the location by all the sides of the box
            heightDivision[0] = (int)(location.Height / box.Length);
            heightDivision[1] = (int)(location.Height / box.Width);
            heightDivision[2] = (int)(location.Height / box.Height);

            //Calculating the the number of boxes for each case
            // LWH
            cases[0] = lengthDivision[0] * widthDivision[1] * heightDivision[2];
            // LHW
            cases[1] = lengthDivision[0] * widthDivision[2] * heightDivision[1];
            // WLH
            cases[2] = lengthDivision[1] * widthDivision[0] * heightDivision[2];
            // WHL
            cases[3] = lengthDivision[1] * widthDivision[2] * heightDivision[0];
            // HLW
            cases[4] = lengthDivision[2] * widthDivision[0] * heightDivision[1];
            // HWL
            cases[5] = lengthDivision[2] * widthDivision[1] * heightDivision[0];

            //Finding the index of the BestWay
            int indexBestWay = Array.IndexOf(cases, cases.Max());
            return (BoxWay)indexBestWay;
        }

        private string ShowBestWayCalcutions(Location location,Box box, BoxWay boxWay)
        {
            string calculations = "";
            switch (boxWay)
            {
                case BoxWay.LWH:
                    calculations = CalculationSteps(location,box.Length, box.Width, box.Height);
                    break;
                case BoxWay.LHW:
                    calculations = CalculationSteps(location, box.Length, box.Height, box.Width);
                    break;
                case BoxWay.WLH:
                    calculations = CalculationSteps(location, box.Width, box.Length, box.Height);
                    break;
                case BoxWay.WHL:
                    calculations = CalculationSteps(location, box.Width, box.Height, box.Length);
                    break;
                case BoxWay.HLW:
                    calculations = CalculationSteps(location, box.Height, box.Length, box.Width);
                    break;
                case BoxWay.HWL:
                    calculations = CalculationSteps(location, box.Height, box.Width, box.Length);
                    break;
            }

            return calculations;
        }

        private string CalculationSteps(Location location,double side1, double side2, double side3)
        {
            string calculationSteps = "";
            calculationSteps += string.Format("{0} ÷ {1} = {2} go back\n", location.Length, side1, (int)(location.Length / side1));
            calculationSteps += string.Format("{0} ÷ {1} = {2} go across\n", location.Width, side2, (int)(location.Width / side2));
            calculationSteps += string.Format("{0} ÷ {1} = {2} go up\n", location.Height, side3, (int)(location.Height / side3));
            return calculationSteps;
        }

        public int HowManyCanFit(Location location, Box box, BoxWay boxWay)
        {
            int howMany = 0;
            switch (boxWay)
            {
                case BoxWay.LWH:
                    howMany = CalculationNumber(location,box.Length, box.Width, box.Height);
                    break;
                case BoxWay.LHW:
                    howMany = CalculationNumber(location,box.Length, box.Height, box.Width);
                    break;
                case BoxWay.WLH:
                    howMany = CalculationNumber(location,box.Width, box.Length, box.Height);
                    break;
                case BoxWay.WHL:
                    howMany = CalculationNumber(location,box.Width, box.Height, box.Length);
                    break;
                case BoxWay.HLW:
                    howMany = CalculationNumber(location,box.Height, box.Length, box.Width);
                    break;
                case BoxWay.HWL:
                    howMany = CalculationNumber(location,box.Height, box.Width, box.Length);
                    break;
            }
            return howMany;
        }

        private int CalculationNumber(Location location, double side1, double side2, double side3)
        {
            int temp = 0;
            temp = (int)(location.Length / side1) * (int)(location.Width / side2) * (int)(location.Height / side3);
            return temp;
        }

        public Location InchesLeft(Location location,Box box, BoxWay boxWay)
        {
            Location temp = new Location();
            switch (boxWay)
            {
                case BoxWay.LWH:
                    temp.Length = location.Length - (int)(location.Length / box.Length) * box.Length;
                    temp.Width = location.Width - (int)(location.Width / box.Width) * box.Width;
                    temp.Height = location.Height - (int)(location.Height / box.Height) * box.Height;
                    break;
                case BoxWay.LHW:
                    temp.Length = location.Length - (int)(location.Length / box.Length) * box.Length;
                    temp.Width = location.Width - (int)(location.Width / box.Height) * box.Height;
                    temp.Height = location.Height - (int)(location.Height / box.Width) * box.Width;
                    break;
                case BoxWay.WLH:
                    temp.Length = location.Length - (int)(location.Length / box.Width) * box.Width;
                    temp.Width = location.Width - (int)(location.Width / box.Length) * box.Length;
                    temp.Height = location.Height - (int)(location.Height / box.Height) * box.Height;
                    break;
                case BoxWay.WHL:
                    temp.Length = location.Length - (int)(location.Length / box.Width) * box.Width;
                    temp.Width = location.Width - (int)(location.Width / box.Height) * box.Height;
                    temp.Height = location.Height - (int)(location.Height / box.Length) * box.Length;
                    break;
                case BoxWay.HLW:
                    temp.Length = location.Length - (int)(location.Length / box.Height) * box.Height;
                    temp.Width = location.Width - (int)(location.Width / box.Length) * box.Length;
                    temp.Height = location.Height - (int)(location.Height / box.Width) * box.Width;
                    break;
                case BoxWay.HWL:
                    temp.Length = location.Length - (int)(location.Length / box.Height) * box.Height;
                    temp.Width = location.Width - (int)(location.Width / box.Width) * box.Width;
                    temp.Height = location.Height - (int)(location.Height / box.Length) * box.Length;
                    break;
            }
            return temp;
        }
    }
}