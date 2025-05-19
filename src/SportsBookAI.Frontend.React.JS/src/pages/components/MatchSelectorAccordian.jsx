import React, { useState, useEffect } from 'react'
import axios from 'axios'
import exportToCSV from '../../utilities/exportToCSV'
import Accordion from 'react-bootstrap/Accordion'
import Button from 'react-bootstrap/Button'
import ButtonGroup from 'react-bootstrap/ButtonGroup'
import Form from 'react-bootstrap/Form'
import PredictionsTable from './PredictionsTable'

function MatchSelectorAccordian(props) {
    const API_URL = import.meta.env.VITE_API_URL
    const [matchesThatNeedPredictions, setMatchesThatNeedPredictions] = useState([])
    const [selectedMatch, setSelectedMatch] = useState('')
    const [loadingPredictions, setLoadingPredictions] = useState(false)
    const [predictions, setPredictions] = useState([])
    const [leagueName, setLeagueName] = useState('')

    useEffect(() => {
        setMatchesThatNeedPredictions(props.matches)
        setLeagueName(props.leagueName)
    }, [props.matches, props.leagueName])

    async function getPredictions() {
        setPredictions([])
        setLoadingPredictions(true)
        const getPrdictions = await axios.post(`${API_URL}/Aggregator/getPredictions`, {
            LeagueName: leagueName,
            MatchId: selectedMatch
        })
        setLoadingPredictions(false)
        setPredictions(getPrdictions["data"])
    }

    if (matchesThatNeedPredictions.length == 0) {
        return null
    }

    return <Accordion className="mb-5">
        <Accordion.Item eventKey="0">
            <Accordion.Header>Simulate Matchup</Accordion.Header>
            <Accordion.Body>
                <Form>
                    <Form.Group className="mb-1" controlId="matchup.Match">
                        <Form.Label>Match</Form.Label>
                        <Form.Select aria-label="Pick Match" value={selectedMatch} onChange={(e) => {
                            setSelectedMatch(e.target.value)
                            setPredictions([])
                        }}>
                            <option value="">Pick A Match</option>
                            {matchesThatNeedPredictions.map((match, index) => (
                                <option key={`home-team-option-${index}`} value={match["id"]}>{match["display"]}</option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                    <ButtonGroup aria-label="Basic example">
                        <Button variant="primary" className="mt-2" disabled={selectedMatch === '' || loadingPredictions} onClick={async (e) => {
                            e.preventDefault()
                            await getPredictions()
                        }}>
                            Get Predictions
                        </Button>
                        <Button variant="primary" className="mt-2" disabled={selectedMatch === '' || loadingPredictions || predictions.length === 0} onClick={(e) => {
                            e.preventDefault()
                            exportToCSV(predictions, `${leagueName} Predictions.csv`)
                        }}>
                            Export Predictions
                        </Button>
                    </ButtonGroup>
                </Form>
                <PredictionsTable predictions={predictions} />
            </Accordion.Body>
        </Accordion.Item>
    </Accordion>
}

export default MatchSelectorAccordian