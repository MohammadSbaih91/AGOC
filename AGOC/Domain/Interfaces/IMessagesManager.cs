using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Domain.Interfaces
{
    public interface IMessagesManager
    {
        // Sends a message and logs it in the system
        Task<OperationResult> SendMessageAsync(MessageViewModel messageViewModel);

        // Retrieves all non-deleted messages
        Task<IEnumerable<MessageViewModel>> GetAllMessagesAsync();

        // Retrieves a specific message by ID
        Task<MessageViewModel> GetMessageByIdAsync(int id);

        // Updates an existing message
        Task<OperationResult> UpdateMessageAsync(MessageViewModel messageViewModel);

        // Soft deletes a message by ID
        Task<OperationResult> DeleteMessageAsync(int id);
    }
}
