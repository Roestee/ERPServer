namespace ERPServer.Domain.Dtos
{
	public record OrderDetailDto(
		Guid ProductId,
		decimal Quantity,
		decimal UnitPrice);
}

