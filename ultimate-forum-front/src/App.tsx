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
					<div class={"grid xl:grid-cols-12 gap-3 items-start"}>
						<table class={"table h-auto xl:col-span-8 table-md border border-base-content/5"}>
							<thead>
							<tr>
								<td style={{width: "100%"}}>Board Name</td>

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
											<A href={`/board/${x.id}`} class={"p-3"} style={{display: "table-cell"}}>
												{x.name}<br/>
												<span class={"text-base-content/60"}>{x.description}</span>
											</A>
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
						<div class={"card items-center xl:col-span-4 w-full p-5 bg-base-200 flex flex-col gap-2"}>
							<div class={"w-full"}>登录</div>
							<input class={"input"} placeholder={"用户名"}/>
							<input class={"input"} type={"password"} placeholder={"密码"}/>
								<div class={"flex  w-auto gap-2"}>
									<button class={"btn btn-primary grow "}>登录</button>
									<button class={"btn btn-secondary grow"}>注册</button>
								</div>
							</div>

					</div>

			  </div>
		  </>


	  </Suspense>
  );
};

export default App;
