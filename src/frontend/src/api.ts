import { ItemResponse } from "./models/ItemResponse";

const baseUrl = "https://localhost:7245";

export async function getItems(
  steamId: string,
  useMockResponse: boolean
): Promise<ItemResponse[]> {
  try {
    const response = await fetch(
      `${baseUrl}/items/${steamId}?` +
        new URLSearchParams({
          useMockResponse: useMockResponse ? "true" : "false",
        })
    );

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
