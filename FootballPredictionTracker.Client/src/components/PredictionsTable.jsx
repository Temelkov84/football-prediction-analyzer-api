function formatKickoffTime(kickoffTime) {
    if (!kickoffTime) {
        return '-'
    }

    return new Date(kickoffTime).toLocaleString('en-GB', {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
    })
}

function getLeagueName(prediction) {
    return prediction.leagueName || prediction.league || 'Unknown League'
}

function getCountryName(prediction) {
    return prediction.country || prediction.countryName || prediction.leagueCountry || '-'
}

function getCountryFlagClass(countryName) {
    const flagClasses = {
        England: 'league-flag-england',
        Germany: 'league-flag-germany',
        Spain: 'league-flag-spain',
        Italy: 'league-flag-italy',
        France: 'league-flag-france',
        Netherlands: 'league-flag-netherlands',
    }

    return flagClasses[countryName] || ''
}

function groupPredictionsByLeague(predictions) {
    return predictions.reduce((groups, prediction) => {
        const leagueName = getLeagueName(prediction)
        const countryName = getCountryName(prediction)
        const groupKey = `${countryName}-${leagueName}`

        if (!groups[groupKey]) {
            groups[groupKey] = {
                countryName,
                leagueName,
                predictions: [],
            }
        }

        groups[groupKey].predictions.push(prediction)

        return groups
    }, {})
}

function getProbabilityStrengthClass(probability) {
    if (probability >= 70) {
        return 'probability-strength-8'
    }

    if (probability >= 60) {
        return 'probability-strength-7'
    }

    if (probability >= 50) {
        return 'probability-strength-6'
    }

    if (probability >= 40) {
        return 'probability-strength-5'
    }

    if (probability >= 30) {
        return 'probability-strength-4'
    }

    if (probability >= 20) {
        return 'probability-strength-3'
    }

    if (probability >= 10) {
        return 'probability-strength-2'
    }

    return 'probability-strength-1'
}

function getLeagueDisplayOrder(group) {
    const displayOrder = {
        'England-Premier League': 1,
        'England-Championship': 2,
        'Spain-La Liga': 3,
        'Germany-Bundesliga': 4,
        'Italy-Serie A': 5,
        'France-Ligue 1': 6,
        'Netherlands-Eredivisie': 7,
    }

    const key = `${group.countryName}-${group.leagueName}`

    return displayOrder[key] || 999
}

function PredictionsTable({ predictions }) {
    if (!predictions || predictions.length === 0) {
        return <p>No predictions available for this week.</p>
    }

    const groupedPredictions = groupPredictionsByLeague(predictions)

    const predictionGroups = Object.values(groupedPredictions).sort((firstGroup, secondGroup) =>
        getLeagueDisplayOrder(firstGroup) - getLeagueDisplayOrder(secondGroup)
    )

    return (
        <div className="predictions-groups">
            {predictionGroups.map((group) => (
                <section
                    key={`${group.countryName}-${group.leagueName}`}
                    className="league-section"
                >
                    <div className="league-header">
                        <h3 className="league-title">
                            {group.countryName !== '-' && (
                                <span
                                    className={`league-flag ${getCountryFlagClass(group.countryName)}`}
                                ></span>
                            )}

                            <span className="league-title-text">
                                {group.countryName !== '-' ? (
                                    <>
                                        <span className="league-country">{group.countryName}</span>
                                        <span className="league-separator">-</span>
                                        <span className="league-name">{group.leagueName}</span>
                                    </>
                                ) : (
                                    <span className="league-name">{group.leagueName}</span>
                                )}
                            </span>
                        </h3>

                        <span className="match-count">{group.predictions.length} matches</span>
                    </div>

                    <div className="table-wrapper">
                        <table className="predictions-table">
                            <thead>
                                <tr>
                                    <th>Date / Time</th>
                                    <th>Country</th>
                                    <th>League</th>
                                    <th>Match</th>
                                    <th>1</th>
                                    <th>X</th>
                                    <th>2</th>
                                </tr>
                            </thead>

                            <tbody>
                                {group.predictions
                                    .sort((firstPrediction, secondPrediction) =>
                                        new Date(firstPrediction.kickoffTime) -
                                        new Date(secondPrediction.kickoffTime)
                                    )
                                    .map((prediction) => (
                                        <tr key={prediction.id}>
                                            <td>{formatKickoffTime(prediction.kickoffTime)}</td>
                                            <td>{getCountryName(prediction)}</td>
                                            <td>{getLeagueName(prediction)}</td>
                                            <td className="match-cell">
                                                {prediction.homeTeam} vs {prediction.awayTeam}
                                            </td>

                                            <td className="prediction-percent">
                                                <span
                                                    className={`probability-badge ${getProbabilityStrengthClass(
                                                        prediction.homeWinProbability
                                                    )}`}
                                                >
                                                    {prediction.homeWinProbability}%
                                                </span>
                                            </td>

                                            <td className="prediction-percent">
                                                <span
                                                    className={`probability-badge ${getProbabilityStrengthClass(
                                                        prediction.drawProbability
                                                    )}`}
                                                >
                                                    {prediction.drawProbability}%
                                                </span>
                                            </td>

                                            <td className="prediction-percent">
                                                <span
                                                    className={`probability-badge ${getProbabilityStrengthClass(
                                                        prediction.awayWinProbability
                                                    )}`}
                                                >
                                                    {prediction.awayWinProbability}%
                                                </span>
                                            </td>
                                        </tr>
                                    ))}
                            </tbody>
                        </table>
                    </div>
                    <div className="mobile-prediction-cards">
                        {group.predictions
                            .sort((firstPrediction, secondPrediction) =>
                                new Date(firstPrediction.kickoffTime) -
                                new Date(secondPrediction.kickoffTime)
                            )
                            .map((prediction) => (
                                <article
                                    key={`mobile-${prediction.id}`}
                                    className="mobile-prediction-card"
                                >
                                    <div className="mobile-prediction-time">
                                        {formatKickoffTime(prediction.kickoffTime)}
                                    </div>

                                    <div className="mobile-prediction-match">
                                        {prediction.homeTeam} vs {prediction.awayTeam}
                                    </div>

                                    <div className="mobile-probabilities">
                                        <div>
                                            <span className="mobile-probability-label">1</span>
                                            <span
                                                className={`probability-badge ${getProbabilityStrengthClass(
                                                    prediction.homeWinProbability
                                                )}`}
                                            >
                                                {prediction.homeWinProbability}%
                                            </span>
                                        </div>

                                        <div>
                                            <span className="mobile-probability-label">X</span>
                                            <span
                                                className={`probability-badge ${getProbabilityStrengthClass(
                                                    prediction.drawProbability
                                                )}`}
                                            >
                                                {prediction.drawProbability}%
                                            </span>
                                        </div>

                                        <div>
                                            <span className="mobile-probability-label">2</span>
                                            <span
                                                className={`probability-badge ${getProbabilityStrengthClass(
                                                    prediction.awayWinProbability
                                                )}`}
                                            >
                                                {prediction.awayWinProbability}%
                                            </span>
                                        </div>
                                    </div>
                                </article>
                            ))}
                    </div>
                </section>
            ))}
        </div>
    )
}

export default PredictionsTable