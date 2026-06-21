using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("students")]
public class StudentsController : ControllerBase
{
    private static readonly List<Student> Students = new()
    {
        new Student("S-001", "Alice Johnson"),
        new Student("S-002", "Bob Smith"),
        new Student("S-003", "Charlie Brown")
    };

    // GET students/{id} → returns a single student or 404
    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        return student is not null ? Ok(student) : NotFound();
    }

    // GET students/all → returns all students
    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(Students);
    }
}

// Simple record model
public record Student(string Id, string Name);
