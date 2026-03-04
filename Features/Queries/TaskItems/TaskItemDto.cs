namespace ToDoWebAPI.Features.Queries.TaskItems;

public record TaskItemDto(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    int? LabelId,
    string? LabelName
);
