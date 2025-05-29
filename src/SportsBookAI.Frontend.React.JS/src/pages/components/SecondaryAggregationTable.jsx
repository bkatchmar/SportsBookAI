import React, { useState, useEffect } from 'react'
import Skeleton from 'react-loading-skeleton'
import Table from 'react-bootstrap/Table'

function SecondaryAggregationTable(props) {
    const [aggregatorData, setAggregatorData] = useState({})
    const displayPercent = (percent) => (percent) ? `${(percent * 100).toFixed(2)}%` : '0%'

    useEffect(() => {
        setAggregatorData(props.aggregatorData)
    }, [props.aggregatorData])

    if (Object.keys(aggregatorData).length === 0) {
        return <Skeleton count={10} />
    }

    return <Table responsive striped hover className="mb-5">
        <thead>
            <tr>
                <th>Highest Over Hit</th>
                <th>Lowest Over Hit</th>
                <th>Average Over Hit</th>
                <th>Highest Under Hit</th>
                <th>Lowest Under Hit</th>
                <th>Average Under Hit</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>{aggregatorData["highestOverHit"].toFixed(2)}</td>
                <td>{aggregatorData["lowestOverHit"].toFixed(2)}</td>
                <td>{aggregatorData["averageOverHit"].toFixed(2)}</td>
                <td>{aggregatorData["highestUnderHit"].toFixed(2)}</td>
                <td>{aggregatorData["lowestUnderHit"].toFixed(2)}</td>
                <td>{aggregatorData["averageUnderHit"].toFixed(2)}</td>
            </tr>
        </tbody>
    </Table>
}

export default SecondaryAggregationTable