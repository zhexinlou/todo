namespace ToDoWebAPI.DTOs;

public record TaskItemCreateDto(
    string Title,
    string Description,
    DateTime DueDate,
    int? LabelId
);
