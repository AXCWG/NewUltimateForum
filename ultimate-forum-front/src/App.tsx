import {Component, createEffect, createResource, createSignal, on, Show, Suspense} from 'solid-js';
import {CutByEllipsis, UltimateForum} from "./SIngleton";
import {A} from "@solidjs/router";
import {For} from "solid-js"
import Enumerable from "linq";

const App: Component = () => {
	const [userInfo, {refetch, mutate}] = createResource(async ()=>{
		return (await UltimateForum.getSchemaV1Index({
			credentials: "include"
		})).data
	})
	const m = window.matchMedia("(width >= 48rem)")
	const [signal_evf87, set_signal_evf87] = createSignal<boolean>(m.matches)
	m.addEventListener('change', ()=>{
		set_signal_evf87(m.matches)
	});
	createEffect(on(signal_evf87, ()=>{
		console.log(signal_evf87())
	}))
	const hiddenCell = "md:table-cell hidden"
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
			  <div class={"lg:mx-50 mx-10 mt-10"}>
				  <div class={"mb-5"}>
					  <div class={"text-6xl font-light"}>Ultimate Forum</div>
					  <div class={"text-base-content/60"}>Good forum software. </div>
				  </div>

				  <table class={"table table-sm border border-base-content/5 table-fixed"}>
					  <thead>
					  	<tr>
							<td>Board Name</td>

							<td>Newest Activity</td>
							<td class={hiddenCell}>Post-Reply count</td>
							<td>Operator</td>
						</tr>
					  </thead>
					  <tbody>
					  <For each={userInfo()?.boards}>
						  {
							  x=>{
								  return <tr>
									  <td>
										  {x.name}<br/>
										  <span class={"text-base-content/60"}>{x.description}</span>
									  </td>
									  <td>{x.posts && x.posts.length !== 0 ? (()=> {
											  let t = Enumerable.from(x.posts).orderByDescending(p => p.createdAt).first();
											  return <Show when={signal_evf87()} fallback={<A href={`/post/${t.id}`}>{CutByEllipsis(t.title)}</A>}><A href={`/post/${t.id}`}>{t.title}</A></Show>
									  })() : <>No activity.</>}</td>
									  <td class={hiddenCell}>{x.posts ? x.posts.length + Enumerable.from(x.posts).sum(p=>p.replies ? p.replies.length : 0) : null}</td>
									  <td>{x.op?.username}</td>
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
