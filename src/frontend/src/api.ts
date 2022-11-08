import { ItemResponse } from "./models/ItemResponse";

const baseUrl = 'https://localhost:7245';

export async function getItems(): Promise<ItemResponse[]> {
	try {
		const response = await fetch(`${baseUrl}/items`);

		if (!response.ok) {
			throw new Error(`HTTP error: ${response.status} ${response.statusText}`);
		}

		const items: ItemResponse[] = await response.json();
		return items;
	} catch (error) {
		console.error(`Failed to fetch items: ${error}`);
	}
	
	return [];
}