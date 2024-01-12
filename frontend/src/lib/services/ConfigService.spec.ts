import { beforeEach, vi } from 'vitest';
import { getFetchMock } from '../testHelpers/getFetchMock';
import ConfigService, { type ApplicationConfig } from './ConfigService';
import { waitFor } from '@testing-library/svelte';
import MessageBus from '../bus/MessageBus';
import { Messages } from '../bus/Messages';

describe('ConfigService', () => {
	describe('initializing', () => {
		let mockFetch = vi.fn();

		beforeEach(async () => {
			const exampleConfig: ApplicationConfig = {
				key: 'value'
			};

			mockFetch = getFetchMock(exampleConfig);

			ConfigService.initialize();
		});

		it('calls fetch for the correct url', async () => {
			await waitFor(() => {
				expect(mockFetch).toHaveBeenCalled();

				let url = mockFetch.mock.calls[0][0];

				expect(url).toContain('/config/config.json');
			});
		});

		it('passes the config into the message bus', async () => {
			await waitFor(() => {
				let lastMessage = MessageBus.getLastMessage<ApplicationConfig>(Messages.ApplicationConfig);

				expect(lastMessage.key).toEqual('value');
			});
		});
	});

	describe('getting config from the service', () => {
		let service: ConfigService;

		beforeEach(() => {
			MessageBus.sendMessage(Messages.ApplicationConfig, { key: 'value' });

			service = new ConfigService();
		});

		it('can get the config from the message bus', () => {
			let result = service.getConfig('key');

			expect(result).toEqual('value');
		});
	});
});
