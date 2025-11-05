<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
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

				<FundName>
					<xsl:value-of select="'Fund Name'"/>
				</FundName>

				<Side>
					<xsl:value-of select="'Side'"/>
				</Side>

				<TransactionType>
					<xsl:value-of select="'TransactionType'"/>
				</TransactionType>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<TickerSymbol>
					<xsl:value-of select="'TickerSymbol'"/>
				</TickerSymbol>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

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
						<FundName>
							<xsl:value-of select="AccountName"/>
						</FundName>

						<Side>
							<xsl:value-of select="Side"/>
						</Side>

					
						
						<!--CSClosingPx	Cash Settle At Closing Date Spot PX
						CSCost	Cash Settle At Cost
						CSSwp	Swap Expire
						CSSwpRl	Swap Expire and Rollover
						CSZero	Cash Settle At Zero Price
						DLCost	Deliver FX At Cost
						DLCostAndPNL	Deliver FX At Cost, PNL At Closing Date Spot FX-->
						
						
						
					
						
						<TransactionType>
							<xsl:choose>
								<xsl:when test ="TransactionType = 'LongAddition'">
									<xsl:value-of select="'Long Addition'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'LongWithdrawal'">
									<xsl:value-of select="'Long Withdrawal'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'ShortAddition'">
									<xsl:value-of select="'Short Addition'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'ShortWithdrawal'">
									<xsl:value-of select="'Short Withdrawal'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'BuytoClose'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'BuytoOpen'">
									<xsl:value-of select="'Buy to Open'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'Sellshort' or TransactionType = 'SellShort'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'SelltoClose'">
									<xsl:value-of select="'Sell to Close'"/>
								</xsl:when>
								<xsl:when test ="TransactionType = 'SelltoOpen'">
									<xsl:value-of select="'Sell to Open'"/>									
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="TransactionType"/>
								</xsl:otherwise>
							</xsl:choose>										
						</TransactionType>

						<Quantity>
							<xsl:value-of select="TotalQty"/>
						</Quantity>

						<Price>
							<xsl:value-of select="AveragePrice"/>
						</Price>

						<TickerSymbol>
							<xsl:value-of select="Symbol"/>
						</TickerSymbol>

						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
			
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>