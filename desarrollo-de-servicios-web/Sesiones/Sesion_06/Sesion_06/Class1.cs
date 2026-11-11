using System.Collections.Generic;

public record NamedResourceDto
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public record AbilityDto
{
    public bool IsHidden { get; set; }
    public int Slot { get; set; }
    public NamedResourceDto Ability { get; set; }
}

public record PastAbilityDto
{
    public NamedResourceDto Generation { get; set; }
    public List<AbilityDto> Abilities { get; set; }
}

public record GameIndexDto
{
    public int GameIndex { get; set; }
    public NamedResourceDto Version { get; set; }
}

public record VersionDetailDto
{
    public int Rarity { get; set; }
    public NamedResourceDto Version { get; set; }
}

public record HeldItemDto
{
    public NamedResourceDto Item { get; set; }
    public List<VersionDetailDto> VersionDetails { get; set; }
}

public record VersionGroupDetailDto
{
    public int LevelLearnedAt { get; set; }
    public NamedResourceDto VersionGroup { get; set; }
    public NamedResourceDto MoveLearnMethod { get; set; }
    public int? Order { get; set; }
}

public record MoveDto
{
    public NamedResourceDto Move { get; set; }
    public List<VersionGroupDetailDto> VersionGroupDetails { get; set; }
}

public record CriesDto
{
    public string Latest { get; set; }
    public string Legacy { get; set; }
}

public record StatDto
{
    public int BaseStat { get; set; }
    public int Effort { get; set; }
    public NamedResourceDto Stat { get; set; }
}

public record PastStatDto
{
    public NamedResourceDto Generation { get; set; }
    public List<StatDto> Stats { get; set; }
}

public record TypeDto
{
    public int Slot { get; set; }
    public NamedResourceDto Type { get; set; }
}

public record PokemonDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BaseExperience { get; set; }
    public int Height { get; set; }
    public bool IsDefault { get; set; }
    public int Order { get; set; }
    public int Weight { get; set; }
    public List<AbilityDto> Abilities { get; set; }
    public List<PastAbilityDto> PastAbilities { get; set; }
    public List<NamedResourceDto> Forms { get; set; }
    public List<GameIndexDto> GameIndices { get; set; }
    public List<HeldItemDto> HeldItems { get; set; }
    public string LocationAreaEncounters { get; set; }
    public List<MoveDto> Moves { get; set; }
    public NamedResourceDto Species { get; set; }
    public Dictionary<string, object> Sprites { get; set; }
    public CriesDto Cries { get; set; }
    public List<StatDto> Stats { get; set; }
    public List<PastStatDto> PastStats { get; set; }
    public List<TypeDto> Types { get; set; }
    public List<object> PastTypes { get; set; }
}