import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';


export interface PredictionResult {
    results: number[];
    predictedNumber: number;
    success: boolean;
    errorMessage: string;
    debugText: string;
}

export interface NumberPredictState {
    loading: boolean;
    loaded: boolean;
    results: number[];
    numberPredicted: number;
    predictResult: boolean;
    errorMessage: string;
    debugText: string;
}

interface PredictImageNumberActionLoading { type: 'PREDICT_IMAGE_LOADING' }

interface PredictImageNumberActionLoaded {
    type: 'PREDICT_IMAGE_LOADED';
    results: number[];
    numberPredicted: number;
    predictResult: boolean;
    errorMessage: string;
    debugText: string;
}

type KnownAction = PredictImageNumberActionLoading | PredictImageNumberActionLoaded;


export const actionCreators = {
    tryPredictNumber: (imageData: any): AppThunkAction<KnownAction> => (dispatch, getState) => {

        let fetchTask = fetch('api/MnistDeep/PredictNumber', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: '{ "imageData": "' + imageData + '" }'
            })
            .then(response => response.json() as Promise<PredictionResult>)
            .then(data => {
                dispatch({ type: 'PREDICT_IMAGE_LOADED', results: data.results, numberPredicted: data.predictedNumber, predictResult: data.success, errorMessage: data.errorMessage, debugText: data.debugText });
                });

            addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
            dispatch({ type: 'PREDICT_IMAGE_LOADING' });
    }
};

export const reducer: Reducer<NumberPredictState> = (state: NumberPredictState, action: KnownAction) => {
    switch (action.type) {
        case 'PREDICT_IMAGE_LOADING':
            return { loading: true, loaded: false } as NumberPredictState;
        case 'PREDICT_IMAGE_LOADED':
            return { loaded: true, loading: false, numberPredicted: action.numberPredicted, results: action.results, predictResult: action.predictResult, errorMessage: action.errorMessage, debugText: action.debugText  } as NumberPredictState;
    }

    return state || { loading: false, loaded: false } as NumberPredictState;
};
