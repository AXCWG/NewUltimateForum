import {Component, createResource, Show, Suspense} from 'solid-js';
import {A, useParams} from "@solidjs/router";
import {For} from "solid-js"
import {UltimateForum} from "./SIngleton";

const BoardPage: Component = () => {
	const params = useParams();

	const [data] = createResource(async () => {
		const id = params.id;
		if (!id) return { board: null, posts: [] };
		const [boardResult, postsResult] = await Promise.all([
			UltimateForum.getSchemaV1Index({
				credentials: "include"
			}),
			UltimateForum.getApiV1PostGetPostUnder({
				query: { boardId: id }
			})
		]);
		const board = boardResult.data?.boards.find(b => b.id == id);
		return { board, posts: postsResult.data ?? [] };
	});

	return (
		<Suspense>
			<>
				<div class={"border-b-1 flex justify-between border-t-1 p-2 border-gray-300 px-5"}>
					<div class={"font-bold"}>Ultimate Forum</div>
					<div class={"flex gap-3"}>
						<A href={"/"}>Home</A>
						<A href={"/login"}>Login</A>
						<A href={"/register"}>Register</A>
					</div>
				</div>
				<div class={"lg:mx-50 mx-10 mt-10"}>
					<div class={"mb-5"}>
						<div class={"text-6xl font-light"}>{data()?.board?.name ?? "Loading..."}</div>
						<div class={"text-base-content/60"}>{data()?.board?.description}</div>
					</div>
					<div class={"grid xl:grid-cols-12 gap-3 items-start"}>
						<table class={"table xl:col-span-8 table-md border border-base-content/5"}>
							<thead>
							<tr>
								<td style={{width: "100%"}}>Post Title</td>
								<td>Author</td>
								<td>Replies</td>
								<td>Created</td>
							</tr>
							</thead>
							<tbody>
							<Show when={(data()?.posts?.length ?? 0) > 0} fallback={
								<tr>
									<td colspan={4} class={"text-center text-base-content/60 p-5"}>No posts in this board yet.</td>
								</tr>
							}>
								<For each={data()?.posts}>
									{x => (
										<tr>
											<td>
												<A href={`/post/${x.id}`} class={"p-3"} style={{display: "table-cell"}}>
													{x.title}
												</A>
											</td>
											<td>{x.poster?.username}</td>
											<td>{x.replies?.length ?? 0}</td>
											<td>{x.createdAt ? new Date(x.createdAt).toLocaleDateString() : ""}</td>
										</tr>
									)}
								</For>
							</Show>
							</tbody>
						</table>
						<div class={"card items-center xl:col-span-4 w-full p-5 bg-base-200 flex flex-col gap-2"}>
							<div class={"w-full font-bold"}>Board Info</div>
							<div class={"w-full text-base-content/60"}>Operator</div>
							<div class={"w-full"}>{data()?.board?.op?.username}</div>
							<div class={"w-full text-base-content/60 mt-2"}>Total Posts</div>
							<div class={"w-full"}>{data()?.posts?.length ?? 0}</div>
						</div>
					</div>
				</div>
			</>
		</Suspense>
	);
};

export default BoardPage;
