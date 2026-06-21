using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("courses")]
public class CoursesController : ControllerBase
{
    private static readonly List<Course> Courses = new()
    {
        new Course("CS-101", "Introduction to Computer Science"),
        new Course("MATH-201", "Calculus II"),
        new Course("ENG-301", "Advanced English Literature")
    };

    // GET courses/{id} → returns a single course or 404
    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var course = Courses.FirstOrDefault(c => c.Code == id);
        return course is not null ? Ok(course) : NotFound();
    }

    // GET courses/all → returns all courses
    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(Courses);
    }
}

// Simple record model
public record Course(string Code, string Title);
