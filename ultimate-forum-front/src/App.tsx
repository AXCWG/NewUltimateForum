import {Component, createResource, Suspense} from 'solid-js';
import {UltimateForum} from "./SIngleton";
import {A} from "@solidjs/router";
import {For} from "solid-js"
import Enumerable from "linq";

const App: Component = () => {
	const [userInfo, {refetch, mutate}] = createResource(async ()=>{
		return (await UltimateForum.getSchemaV1Index({
			credentials: "include"
		})).data
	})

  return (
	  <Suspense>
		  <>
			  <div class={"border-b-1 flex justify-between border-t-1 p-2  border-gray-300 px-5"}>
				  <div class={"font-bold"}>Ultimate Forum</div>
				  <div class={"flex gap-3"}>
					  <A href={"/login"}>Login</A>
					  <A href={"/register"}>Register</A>
				  </div>
			  </div>
			  <div class={"mx-50 mt-10"}>
				  <div class={"text-6xl font-light"}>Ultimate Forum</div>
				  <table class={"table table-md"}>
					  <thead>
					  	<tr>
							<td>Board Name</td>
							<td>Board Desc.</td>
							<td>Newest Activity</td>
							<td>Post-Reply count</td>
							<td>Operator</td>
						</tr>
					  </thead>
					  <tbody>
					  <For each={userInfo()?.boards}>
						  {
							  x=>{
								  return <tr>
									  <td>{x.name}</td>
									  <td>{x.description}</td>
									  <td>{x.posts && x.posts.length !== 0 ? (()=> {
											  let t = Enumerable.from(x.posts).orderByDescending(p => p.createdAt).first();
											  return <A href={`/post/${t.id}`}>{t.title}</A>
									  })() : <>No activity.</>}</td>
									  <td>{x.posts ? x.posts.length + Enumerable.from(x.posts).sum(p=>p.replies ? p.replies.length : 0) : null}</td>
								  </tr>
							  }
						  }
					  </For>
					  </tbody>
				  </table>
			  </div>
		  </>


	  </Suspense>
  );
};

export default App;
