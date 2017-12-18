import * as React from 'react';
import { Route } from 'react-router-dom';
import NumberPredictComponent from './components/NumberPredictComponent';
import { Layout } from './components/Layout';

export const routes = <Layout>
    <Route exact path='/' component={NumberPredictComponent} />
    <Route path='/mnist' component={NumberPredictComponent} />
</Layout>;
