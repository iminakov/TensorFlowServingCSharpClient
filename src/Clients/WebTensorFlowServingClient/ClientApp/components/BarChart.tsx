import * as React from 'react';
import * as BarChart from 'react-chartjs';

type BarChartProps = {
    results: number[];
}

export class BarChartComponent extends React.Component<BarChartProps, {}> {
    public render() {

        return <BarChart.Bar
            data = {{
                labels: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
                datasets: [
                    {
                        data: this.props.results,
                        fillColor: "rgba(220,220,220,0.2)",
                        label: "My First dataset",
                        pointColor: "rgba(220,220,220,1)",
                        pointHighlightFill: "#fff",
                        pointHighlightStroke: "rgba(220,220,220,1)",
                        pointStrokeColor: "#fff",
                        strokeColor: "rgba(220,220,220,1)"
                    }
                ]
            }}
        />
    }
}