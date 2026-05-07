/* @refresh reload */
import './index.css';
import { render } from 'solid-js/web';
import 'solid-devtools';

import App from './App';
import ErrorJsx from "./Error";
import {UltimateForum} from "./SIngleton";
import {Route, Router} from "@solidjs/router";

const root = document.getElementById('root');

if (import.meta.env.DEV && !(root instanceof HTMLElement)) {
  throw new Error(
    'Root element not found. Did you forget to add it to your index.html? Or maybe the id attribute got misspelled?',
  );
}

const connectionCheck = (await UltimateForum.getPing());
if(connectionCheck.response?.ok){
  render(() => <Router>
    <Route path={"/"} component={App}></Route>
  </Router>, root!);
}else{
  render(()=><ErrorJsx ErrorMessage={`Unable to connect to forum server: ${connectionCheck.response?.status},${connectionCheck.response?.statusText}`}/>, root!);
}

