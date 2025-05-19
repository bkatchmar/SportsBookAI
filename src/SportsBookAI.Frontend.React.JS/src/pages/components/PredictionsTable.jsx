import React, { useState, useEffect } from 'react'
import Table from 'react-bootstrap/Table'

function PredictionsTable(props) {
    const [predictions, setPredictions] = useState([])

    useEffect(() => {
        setPredictions(props.predictions)
    }, [props.predictions])

    if (predictions.length === 0) {
        return null
    }

    return <Table responsive striped hover className="mt-1" size="sm">
        <thead>
            <tr>
                <th>Prediction Pattern ID</th>
                <th>Prediction Name</th>
                <th>Text</th>
            </tr>
        </thead>
        <tbody>
            {predictions.map((prediction, index) => (
                <tr key={`prediction-row-${index}`}>
                    <td>{prediction["id"]}</td>
                    <td>{prediction["name"]}</td>
                    <td>{prediction["predictionText"]}</td>
                </tr>
            ))}
        </tbody>
    </Table>
}

export default PredictionsTable