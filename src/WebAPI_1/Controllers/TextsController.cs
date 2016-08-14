using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TextsController : Controller
    {
        private ITextRepository TextItems { set; get; }
        private string ConnectionString;

        public TextsController(ITextRepository textItems)
        {
            TextItems = textItems;
        }

        // GET: api/texts/
        [HttpGet]
        public async Task<dynamic> GetAll()
        {
            return await TextItems.GetAll();
            // returns 200 with item(s) required or NotFound 404.
        }

        //GET: api/texts/{id}
        [HttpGet("{key}", Name = "GetText")]
        public async Task<dynamic> GetByKey(string key)
        {
            return await TextItems.GetByKey(key);
            // returns 200 with item(s) required or NotFound 404.
        }


        // POST api/texts/
        [HttpPost]
        public async Task<dynamic> Create([FromBody]Text newText)
            //[FromBody] attribute tells MVC to get the value from the HTTP request body.
        {
                if (newText == null)
                {
                    BadRequest();
                }
            await TextItems.Create(newText);    // Successful response is 200 - Created and the newly created item..
            return CreatedAtRoute("GetText", new {Controller = "Texts", key = newText.Key }, newText);  // TODO: check out what it returns.
            // 'CreateRoute' method returns a 201 response.
            // It also adds a Location header to the response, which specifies the URI of the newly item.

            ///POST http://localhost:5000/api/texts/ HTTP/1.1
            /// User - Agent: Fiddler
            /// Host: localhost: 5000
            /// Content - Type: application / json
            /// Content - Length: 102
            /// { "Key": "Fiddler Tester 3","Title": "Test 1","PubDate": "2016-06-23T11:11:46.353","Author": "Fiddler"}
        }


        // PUT: api/texts/update/{key}
        [HttpPut("[action]/{key}")]
        public async Task<dynamic> Update(string key, [FromBody]Text text)
        {
            if (text == null || text.Key != key)
                return BadRequest();
            if (TextItems.GetByKey(key) == null)
                return NotFound();
            return await TextItems.Update(text);
            // Successful response is 204 - NonContent.
            // Failure response is 400 - BadRequest.
            // Otherwise should be 500 - Internal Server Error.

            /// PUT http://localhost:5000/api/texts/3 HTTP/1.1
            /// User - Agent: Fiddler
            ///  Host: localhost: 5000
            /// Content - Type: application / json
            /// Content - Length: 97
            /// { "Key": "3","Title": "Testing Update 1","PubDate": "2016-06-23T11:11:46.353","Author": "Fiddler"}
        }
    }
}
