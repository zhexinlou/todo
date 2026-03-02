namespace ToDoWebAPI.DTOs;

public record TaskItemResponseDto(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    int? LabelId,
    string? LabelName
);
