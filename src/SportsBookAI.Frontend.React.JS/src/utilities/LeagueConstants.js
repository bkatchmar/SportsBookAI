const LeagueSpecificFlags = {
    "LEAGUESTHATUSEWEEKS": ["UFL", "NFL"],
    "LEAGUESTHATUSEPITCHERS": ["MLB"]
}

export const DoesThisLeagueUseWeeks = (leagueName) => {
    const processedName = leagueName.toString().toUpperCase().trim()

    for (let i = 0; i < LeagueSpecificFlags["LEAGUESTHATUSEWEEKS"].length; i++) {
        let current = LeagueSpecificFlags["LEAGUESTHATUSEWEEKS"][i].toString().toUpperCase().trim()
        if (current === processedName) return true
    }

    return false
}

export const DoesThisLeagueUsePitchers = (leagueName) => {
    const processedName = leagueName.toString().toUpperCase().trim()

    for (let i = 0; i < LeagueSpecificFlags["LEAGUESTHATUSEPITCHERS"].length; i++) {
        let current = LeagueSpecificFlags["LEAGUESTHATUSEPITCHERS"][i].toString().toUpperCase().trim()
        if (current === processedName) return true
    }

    return false
}