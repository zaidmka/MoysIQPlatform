using Microsoft.AspNetCore.Mvc;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;
using MoysIQPlatform.Server.Services.QuestionsServices;

namespace MoysIQPlatform.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class QuestionsController : ControllerBase
	{
		private readonly IQuestionsServices _questionService;

		public QuestionsController(IQuestionsServices questionService)
		{
			_questionService = questionService;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<QuestionWithEmployeeDto>>>> GetAll()
		{
			var result = await _questionService.GetAllQuestions();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceResponse<Question>>> Get(int id)
		{
			var result = await _questionService.GetQuestionById(id);
			if (!result.Success)
				return NotFound(result);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<QuestionWithEmployeeDto>>> Create(QuestionCreateDto question)
		{
			var result = await _questionService.CreateQuestion(question);
			return Ok(result);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ServiceResponse<Question>>> Update(int id, Question updatedQuestion)
		{
			var result = await _questionService.UpdateQuestion(id, updatedQuestion);
			if (!result.Success)
				return NotFound(result);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<bool>>> Delete(int id)
		{
			var result = await _questionService.DeleteQuestion(id);
			return Ok(result);
		}
	}
}
