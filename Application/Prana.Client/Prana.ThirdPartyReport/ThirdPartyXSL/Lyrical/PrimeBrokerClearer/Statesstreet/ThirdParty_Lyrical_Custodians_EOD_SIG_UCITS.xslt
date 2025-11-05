<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>


				<TradeStatus>
					<xsl:value-of select="'TradeStatus'"/>
				</TradeStatus>


				<ClientReference>
					<xsl:value-of select="'ClientReference'"/>
				</ClientReference>


				<MCF>
					<xsl:value-of select="'MCF Fund Code'"/>
				</MCF>

				
				<OperationCode>
					<xsl:value-of select="'OperationCode'"/>
				</OperationCode>

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

				<AmountFee1>
					<xsl:value-of select="'AmountFee1'"/>
				</AmountFee1>

				<AmountFee2>
					<xsl:value-of select="'AmountFee2'"/>
				</AmountFee2>

				<PSET>
					<xsl:value-of select="'Pset'"/>
				</PSET>

				<StateStreetFundCode>
					<xsl:value-of select="'StateStreetFundCode'"/>
				</StateStreetFundCode>


				<DEAGREAG>
					<xsl:value-of select="'DEAG/REAG'"/>
				</DEAGREAG>




			</ThirdPartyFlatFileDetail>



			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test="AccountName='SSCSIL SIG LYRICAL FUND'">
					<ThirdPartyFlatFileDetail>
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						

						<xsl:variable name = "PRANA_FUND_NAME">
							<xsl:value-of select="AccountName"/>
						</xsl:variable>

						<xsl:variable name ="PB_FUND_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name='Lyrical']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
						</xsl:variable>


						<xsl:variable name="YearTD">
							<xsl:value-of select="substring-after(substring-after(TradeDate,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="MonthTD">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(Date,'/'))=1">
									<xsl:value-of select="concat('0',substring-before(TradeDate,'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(TradeDate,'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="DayTD">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(substring-after(TradeDate,'/'),'/'))=1">
									<xsl:value-of select="concat('0',substring-before(substring-after(TradeDate,'/'),'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="YearSD">
							<xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="MonthSD">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(Date,'/'))=1">
									<xsl:value-of select="concat('0',substring-before(SettlementDate,'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(SettlementDate,'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="DaySD">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(substring-after(SettlementDate,'/'),'/'))=1">
									<xsl:value-of select="concat('0',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						
						<TaxLotState>
							<xsl:value-of select="'NEW'"/>
						</TaxLotState>


						<TradeStatus>
							<xsl:value-of select="'New'"/>
						</TradeStatus>


						<ClientReference>
							<xsl:value-of select="''"/>
						</ClientReference>


						<MCF>
							<xsl:value-of select="''"/>
						</MCF>

						

						<OperationCode>
							<xsl:value-of select="Side"/>
						</OperationCode>

						<SecuritiesInternationalCodes>
							<xsl:value-of select="ISIN"/>
						</SecuritiesInternationalCodes>


						<Counterparty>
							<xsl:choose>
								<xsl:when test="CounterParty='JPM'">
									<xsl:value-of select="'JPMSUS3X'"/>
								</xsl:when>
								<xsl:when test="CounterParty='DBSI'">
									<xsl:value-of select="'NWSCUS33'"/>
								</xsl:when>
								<xsl:when test="CounterParty='BGCE'">
									<xsl:value-of select="'BGCFUS33XXX'"/>
								</xsl:when>
								<xsl:when test="CounterParty='SMHI'">
									<xsl:value-of select="'PRSHUS33'"/>
								</xsl:when>
								<xsl:when test="CounterParty='OPCO'">
									<xsl:value-of select="'OPPYUS33'"/>
								</xsl:when>
							</xsl:choose>

						</Counterparty>

						<TradeDate>
							<xsl:value-of select="concat($YearTD,$MonthTD,$DayTD)"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="concat($YearSD,$MonthSD,$DaySD)"/>
						</SettlementDate>

						<Nominal>
							<xsl:value-of select="AllocatedQty"/>
						</Nominal>

						<QuantityExpression>
							<xsl:value-of select="'UNIT'"/>
						</QuantityExpression>

						<Price>
							<xsl:value-of select="AveragePrice"/>
						</Price>

						<GrossAmount>
							<xsl:value-of select="format-number(GrossAmount,'0.##')"/>
						</GrossAmount>

						<AccruedInterests>
							<xsl:value-of select="''"/>
						</AccruedInterests>

						<SettlementCurrency>
							<xsl:value-of select="'USD'"/>
						</SettlementCurrency>

						<SettlementAmount>
							<xsl:value-of select="format-number(NetAmount,'0.##')"/>
						</SettlementAmount>

						<AmountFee1>
							<xsl:value-of select="format-number(CommissionCharged,'0.##')"/>
						</AmountFee1>

						<AmountFee2>
							<xsl:value-of select="format-number(StampDuty,'0.##')"/>
						</AmountFee2>

						<PSET>
							<xsl:value-of select="'DTCYUS33'"/>
						</PSET>

						<StateStreetFundCode>
							<xsl:value-of select="'PJA2'"/>
						</StateStreetFundCode>


						<DEAGREAG>
							<xsl:choose>
								<xsl:when test="CounterParty='JPM'">
									<xsl:value-of select="'352'"/>
								</xsl:when>
								<xsl:when test="CounterParty='DBSI'">
									<xsl:value-of select="'573'"/>
								</xsl:when>
								<xsl:when test="CounterParty='BGCE'">
									<xsl:value-of select="'161'"/>
								</xsl:when>
								<xsl:when test="CounterParty='OPCO'">
									<xsl:value-of select="'571'"/>
								</xsl:when>
								<xsl:when test="CounterParty='SMHI'">
									<xsl:value-of select="'443'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</DEAGREAG>


						
					</ThirdPartyFlatFileDetail>

				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
