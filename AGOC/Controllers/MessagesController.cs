using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGOC.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessagesManager _messagesManager;
        private readonly IMapper _mapper;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessagesManager messagesManager, IMapper mapper, ILogger<MessagesController> logger)
        {
            _messagesManager = messagesManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var messages = await _messagesManager.GetAllMessagesAsync();
                var totalCount = messages.Count();
                var pagedMessages = messages.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var mappedMessages = _mapper.Map<List<MessageViewModel>>(pagedMessages);

                var pagination = new Pagination<MessageViewModel>(mappedMessages, totalCount, pageNumber, pageSize);
                ViewData["PaginationAction"] = "Index";

                _logger.LogInformation("Successfully retrieved paginated messages.");
                return View(pagination);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated messages.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Fetching message details for ID: {Id}", id);
            var message = await _messagesManager.GetMessageByIdAsync(id);
            if (message == null)
            {
                _logger.LogWarning("Message with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedMessage = _mapper.Map<MessageViewModel>(message);
            _logger.LogInformation("Successfully retrieved message details for ID: {Id}", id);
            return View(mappedMessage);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigated to Create Message view.");
            return View(new MessageViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to create a new message.");
                    var message = _mapper.Map<Message>(messageViewModel);
                    var result = await _messagesManager.SendMessageAsync(messageViewModel);

                    if (result.Success)
                    {
                        _logger.LogInformation("Successfully created a new message.");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create message. Error: {ErrorMessage}", result.ErrorMessage);
                        ViewData["ErrorMessage"] = result.ErrorMessage;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the message.");
                    ViewData["ErrorMessage"] = "An unexpected error occurred.";
                }
            }
            else
            {
                _logger.LogWarning("Model validation failed for message creation.");
            }

            return View(messageViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Fetching message details for editing with ID: {Id}", id);
            var message = await _messagesManager.GetMessageByIdAsync(id);
            if (message == null)
            {
                _logger.LogWarning("Message with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedMessage = _mapper.Map<MessageViewModel>(message);
            _logger.LogInformation("Successfully retrieved message for editing with ID: {Id}", id);
            return View(mappedMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to update message with ID: {Id}.", messageViewModel.MessageID);
                    var result = await _messagesManager.UpdateMessageAsync(messageViewModel);

                    if (result.Success)
                    {
                        _logger.LogInformation("Successfully updated message with ID: {Id}.", messageViewModel.MessageID);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogWarning("Failed to update message with ID: {Id}. Error: {ErrorMessage}", messageViewModel.MessageID, result.ErrorMessage);
                        ViewData["ErrorMessage"] = result.ErrorMessage;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating message with ID: {Id}.", messageViewModel.MessageID);
                    ViewData["ErrorMessage"] = "An unexpected error occurred.";
                }
            }
            else
            {
                _logger.LogWarning("Model validation failed for message update with ID: {Id}.", messageViewModel.MessageID);
            }

            return View(messageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete message with ID: {Id}.", id);
                var result = await _messagesManager.DeleteMessageAsync(id);

                if (result.Success)
                {
                    _logger.LogInformation("Successfully deleted message with ID: {Id}.", id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogWarning("Failed to delete message with ID: {Id}. Error: {ErrorMessage}", id, result.ErrorMessage);
                    ViewData["ErrorMessage"] = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting message with ID: {Id}.", id);
                ViewData["ErrorMessage"] = "An unexpected error occurred.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
