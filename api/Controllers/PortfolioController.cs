using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase 
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepository,
        IPortfolioRepository portfolioRepository)
        {
            this._userManager = userManager;
            this._stockRepository = stockRepository;
            this._portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio(){
            var username = User.GetUsername(); // get current User
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            return Ok(userPortfolio);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol){
            var username = User.GetUsername(); // get current User
            var appUser = await _userManager.FindByNameAsync(username);

            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if(stock == null) return BadRequest("Stock not found");

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e=>e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Cannot add the same stock to Portfolio"); 
            
            var portfolioModel = new Portfolio{
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await _portfolioRepository.CreateAsync(portfolioModel);

            if(portfolioModel == null){
                return StatusCode(500, "Couldn't create");
            }
            else{
                return Created();
            }
            
            }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername(); // get current User
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower())
            .ToList();
            if (filteredStock.Count() == 1){
                await _portfolioRepository.DeletePortfolio(appUser,symbol);
            }
            else{
                return BadRequest("Stock is not in your portfolio");
            }

            return Ok();

        }
    }
}