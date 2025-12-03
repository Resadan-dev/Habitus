namespace Valoron.RpgCore.Application.Dtos;

public record PlayerDto(
    Guid Id,
    int Level,
    int Xp,
    int Strength,
    int Intellect,
    int Stamina);
