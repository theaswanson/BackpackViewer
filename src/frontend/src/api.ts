import { ItemsResponse } from "./models/ItemResponse";

const baseUrl = import.meta.env.VITE_API_URL;

export async function getItems(
  steamId: string,
  useMockResponse: boolean
): Promise<ItemsResponse> {
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

    const items: ItemsResponse = await response.json();
    return items;
  } catch (error) {
    console.error(`Failed to fetch items: ${error}`);
  }

  return { items: [], totalBackpackSlots: 0 };
}
