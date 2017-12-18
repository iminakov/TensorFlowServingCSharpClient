import * as React from 'react';
import * as ReactPaint from 'react-paint';

const props = {
    style: {
        background: '#000000',
    },
    brushCol: '#ffffff',
    lineWidth: 10,
    className: 'react-paint',
    height: 380,
    width: 280
}; 


export class DrawComponent extends React.Component<any, any> {
    
    constructor(props: any) {
        super(props);
        this.clear = this.clear.bind(this);
    }

    public getImageData() {
        let clearing = this.state ? this.state.clearing : false;

        let image = (this.refs["paint_region"] as any).canvas.toDataURL("image/png");
        return image.replace('data:image/png;base64,', '');
    }

    public clear() {
        this.setState({ clearing: this.state != null ? !this.state.clearing : false });
        setTimeout(() => { this.setState({ clearing: this.state != null ? !this.state.clearing : false }) }, 200);
    }

    public render() {
        let clearValue = this.state != null ? this.state.clearing : true;

        if (!clearValue) {
            return (
                <div className="paint_region_container">
                    <div className="paint_region_clear">
                        <br />
                    </div>
                    <div className="paint_region_hack">
                        <br />
                    </div>
                </div>);
        }

        return (
            <div className="paint_region_container">
                <ReactPaint.ReactPaint ref="paint_region" {...props} />
                <div className="paint_region_hack">
                    <br />
                </div>
            </div>);
    }
}