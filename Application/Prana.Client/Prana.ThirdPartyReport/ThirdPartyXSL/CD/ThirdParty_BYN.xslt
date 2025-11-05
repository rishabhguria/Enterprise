<?xml version="1.0" encoding="UTF-8"?>

								<!--
								 Description -		Jef EOD file 
								 Date Created -     02-10-2012(mm-DD-YY)
								-->

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

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<AssetType>
					<xsl:value-of select="'Asset Type'"/>
				</AssetType>

				<Contract>
					<xsl:value-of select="'Contract'"/>
				</Contract>

				<BBGKey>
					<xsl:value-of select="'BBG Key'"/>
				</BBGKey>

				<B_S>
					<xsl:value-of select="'B/S'"/>
				</B_S>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select="'Settle Date'"/>
				</SettleDate>

				<Open_Close>
					<xsl:value-of select="'Open/Close'"/>
				</Open_Close>

				<Long_Desc>
					<xsl:value-of select="'Long Desc'"/>
				</Long_Desc>

				<Cusip>
					<xsl:value-of select="'Cusip'"/>
				</Cusip>

				<Expiry>
					<xsl:value-of select="'Expiry'"/>
				</Expiry>

				<FX>
					<xsl:value-of select="'FX'"/>
				</FX>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>
					
					<!--for system use only-->					
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>
					
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>
					
					<AssetType>
						<xsl:value-of select="translate(Asset,$varLowerCase,$varUpperCase)"/>
					</AssetType>

					<Contract>
						<xsl:value-of select="Symbol"/>
					</Contract>

					<BBGKey>
						<xsl:value-of select="''"/>
					</BBGKey>


					<xsl:variable name="varSide">
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell short' or Side='Sell to Open' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<B_S>
						<xsl:value-of select="$varSide"/>
					</B_S>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<Open_Close>
						<xsl:value-of select="''"/>
					</Open_Close>

					<Long_Desc>
						<xsl:value-of select="FullSecurityName"/>
					</Long_Desc>
	
					<Cusip>
						<xsl:value-of select="''"/>
					</Cusip>

					<xsl:variable name="varExpiry">
						<xsl:choose>
							<xsl:when test="ExpirationDate='01/01/1800'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="ExpirationDate"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Expiry>
						<xsl:value-of select="$varExpiry"/>
					</Expiry>

					<FX>
						<xsl:value-of select="''"/>
					</FX>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
