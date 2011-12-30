using System.Collections.Generic;
using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class GiftRepository : IGiftRepository
    {
        private readonly IConfiguration _configuration;

        public GiftRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Category> GetCategories()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Category.All().Cast<Category>();       
        }

        public IEnumerable<Gift> GetGifts()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);        
            return connection.Gifts.All().Cast<Gift>();           
        } 
    }

}