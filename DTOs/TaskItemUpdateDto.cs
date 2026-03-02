namespace ToDoWebAPI.DTOs;

public record TaskItemUpdateDto(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    int? LabelId
);
