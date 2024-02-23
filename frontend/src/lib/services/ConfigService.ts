import MessageBus from '../bus/MessageBus';
import { Messages } from '../bus/Messages';

export type ApplicationConfig = {
	key: string;
	baseUrl: string;
};

export default class ConfigService {
	static async initialize() {
		await this.getConfigFromFile();
	}

	private static async getConfigFromFile() {
		let config: ApplicationConfig = await fetch('/config/config.json').then((res) => res.json());

		MessageBus.sendMessage(Messages.ApplicationConfig, config);
	}

	getConfig<T>(key: keyof ApplicationConfig): T {
		let currentConfig = MessageBus.getLastMessage<ApplicationConfig>(Messages.ApplicationConfig);

		return currentConfig[key] as T;
	}
}
