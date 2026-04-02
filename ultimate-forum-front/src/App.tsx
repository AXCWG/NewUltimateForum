import type { Component } from 'solid-js';
import ultimateForum from "./static";

const App: Component = () => {
	ultimateForum.Login({Username: "admin", Password: "123asd"}).then(ok=>{
		console.log(ok)
	})
  return (
    <p class="text-4xl text-green-700 text-center py-20">{}</p>
  );
};

export default App;
