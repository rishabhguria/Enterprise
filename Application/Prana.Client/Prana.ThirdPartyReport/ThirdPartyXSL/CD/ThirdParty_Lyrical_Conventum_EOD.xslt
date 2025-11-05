<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<ClientReference>
					<xsl:value-of select="'ClientReference'"/>
				</ClientReference>

				<SenderCode>
					<xsl:value-of select="'SenderCode'"/>
				</SenderCode>

				<OperationCode>
					<xsl:value-of select="'OperationCode'"/>
				</OperationCode>

				<EfaIDCode>
					<xsl:value-of select="'EfaIDCode'"/>
				</EfaIDCode>

				<SecuritiesInternationalCodes>
					<xsl:value-of select="'SecuritiesInternationalCodes'"/>
				</SecuritiesInternationalCodes>

				<Counterparty>
					<xsl:value-of select="'Counterparty'"/>
				</Counterparty>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select="'SettlementDate'"/>
				</SettlementDate>

				<Nominal>
					<xsl:value-of select="'Nominal'"/>
				</Nominal>

				<QuantityExpression>
					<xsl:value-of select="'QuantityExpression'"/>
				</QuantityExpression>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<DealCurrency>
					<xsl:value-of select="'DealCurrency'"/>
				</DealCurrency>

				<GrossAmount>
					<xsl:value-of select="'GrossAmount'"/>
				</GrossAmount>

				<AccruedInterests>
					<xsl:value-of select="'AccruedInterests'"/>
				</AccruedInterests>

				<SettlementCurrency>
					<xsl:value-of select="'SettlementCurrency'"/>
				</SettlementCurrency>

				<SettlementAmount>
					<xsl:value-of select="'SettlementAmount'"/>
				</SettlementAmount>

				<TypeFee1>
					<xsl:value-of select="'TypeFee1'"/>
				</TypeFee1>

				<AmountFee1>
					<xsl:value-of select="'AmountFee1'"/>
				</AmountFee1>

				<TypeFee2>
					<xsl:value-of select="'TypeFee2'"/>
				</TypeFee2>

				<AmountFee2>
					<xsl:value-of select="'AmountFee2'"/>
				</AmountFee2>

				<TypeFee3>
					<xsl:value-of select="'TypeFee3'"/>
				</TypeFee3>

				<AmountFee3>
					<xsl:value-of select="'AmountFee3'"/>
				</AmountFee3>

				<TypeFee4>
					<xsl:value-of select="'TypeFee4'"/>
				</TypeFee4>

				<AmountFee4>
					<xsl:value-of select="'AmountFee4'"/>
				</AmountFee4>

				<TypeFee5>
					<xsl:value-of select="'TypeFee5'"/>
				</TypeFee5>

				<AmountFee5>
					<xsl:value-of select="'AmountFee5'"/>
				</AmountFee5>

				<NewCancel>
					<xsl:value-of select="'NewCancel'"/>
				</NewCancel>

				<TradePlace>
					<xsl:value-of select="'TradePlace'"/>
				</TradePlace>

				<Pset>
					<xsl:value-of select="'Pset'"/>
				</Pset>

				<OperationFreeOfPayment>
					<xsl:value-of select="'OperationFreeOfPayment'"/>
				</OperationFreeOfPayment>

				<BuyerSellerAccount>
					<xsl:value-of select="'Buyer/SellerAccount'"/>
				</BuyerSellerAccount>

				<DEAGREAG>
					<xsl:value-of select="'DEAG/REAG'"/>
				</DEAGREAG>

				<BeneficiaryCashAccount>
					<xsl:value-of select="'BeneficiaryCashAccount'"/>
				</BeneficiaryCashAccount>

				<DECURECU>
					<xsl:value-of select="'DECU/RECU'"/>
				</DECURECU>

				<!-- system use only-->

				<FromDeleted>
					<xsl:value-of select ="'FromDeleted'"/>
				</FromDeleted>
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<ClientReference>
						<xsl:value-of select="''"/>
					</ClientReference>

					<SenderCode>
						<xsl:value-of select="'LYAMUSGE'"/>
					</SenderCode>

					<OperationCode>
						<xsl:value-of select="substring-before(Side,' ')"/>
					</OperationCode>

					<EfaIDCode>
						<xsl:value-of select="AccountMappedName"/>
					</EfaIDCode>

					<SecuritiesInternationalCodes>
						<xsl:value-of select="Symbol"/>
					</SecuritiesInternationalCodes>

					<Counterparty>
						<xsl:value-of select="'JPMSUS3X'"/>
					</Counterparty>

					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>
					</SettlementDate>

					<Nominal>
						<xsl:value-of select="AllocatedQty"/>
					</Nominal>

					<QuantityExpression>
						<xsl:value-of select="'Unit'"/>
					</QuantityExpression>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<DealCurrency>
						<xsl:value-of select="'USD'"/>
					</DealCurrency>

					<GrossAmount>
						<xsl:value-of select="AllocatedQty*AveragePrice"/>
					</GrossAmount>

					<AccruedInterests>
						<xsl:value-of select="''"/>
					</AccruedInterests>

					<SettlementCurrency>
						<xsl:value-of select="'USD'"/>
					</SettlementCurrency>

					<SettlementAmount>
						<xsl:value-of select="NetAmount"/>
					</SettlementAmount>

					<TypeFee1>
						<xsl:value-of select="'EXEC'"/>
					</TypeFee1>

					<AmountFee1>
						<xsl:value-of select="''"/>
					</AmountFee1>

					<TypeFee2>
						<xsl:value-of select="''"/>
					</TypeFee2>

					<AmountFee2>
						<xsl:value-of select="''"/>
					</AmountFee2>

					<TypeFee3>
						<xsl:value-of select="''"/>
					</TypeFee3>

					<AmountFee3>
						<xsl:value-of select="''"/>
					</AmountFee3>

					<TypeFee4>
						<xsl:value-of select="''"/>
					</TypeFee4>

					<AmountFee4>
						<xsl:value-of select="''"/>
					</AmountFee4>

					<TypeFee5>
						<xsl:value-of select="''"/>
					</TypeFee5>

					<AmountFee5>
						<xsl:value-of select="''"/>
					</AmountFee5>

					<NewCancel>
						<xsl:value-of select="''"/>
					</NewCancel>

					<TradePlace>
						<xsl:value-of select="'EXCH/XNYS'"/>
					</TradePlace>

					<Pset>
						<xsl:value-of select="'DTCYUS33'"/>
					</Pset>

					<OperationFreeOfPayment>
						<xsl:value-of select="''"/>
					</OperationFreeOfPayment>

					<BuyerSellerAccount>
						<xsl:value-of select="''"/>
					</BuyerSellerAccount>

					<DEAGREAG>
						<xsl:value-of select="'352'"/>
					</DEAGREAG>

					<BeneficiaryCashAccount>
						<xsl:value-of select="''"/>
					</BeneficiaryCashAccount>

					<DECURECU>
						<xsl:value-of select="''"/>
					</DECURECU>

					<!-- system use only-->

					<FromDeleted>
						<xsl:value-of select ="FromDeleted"/>
					</FromDeleted>
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
