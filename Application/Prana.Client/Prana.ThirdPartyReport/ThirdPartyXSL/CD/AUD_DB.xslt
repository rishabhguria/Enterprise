<?xml version="1.0" encoding="UTF-8"?>
<!-- Description: Omgeo Integration, Created Date: 06-15-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>




	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="StringToNumber">
		<xsl:param name="stringValue"/>
		<xsl:choose>
			<xsl:when test="contains($stringValue,'E') or contains($stringValue,'e')">
				<xsl:variable name="vExponent" select="substring-after($stringValue,'E')"/>
				<xsl:variable name="vMantissa" select="substring-before($stringValue,'E')"/>
				<xsl:variable name="vFactor"
					 select="substring('100000000000000000000000000000000000000000000',
                               1, substring($vExponent,2) + 1)"/>
				<xsl:choose>
					<xsl:when test="starts-with($vExponent,'-')">
						<xsl:value-of select="$vMantissa div $vFactor"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$vMantissa * $vFactor"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$stringValue"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

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
				
				
				<!--1-->
				<BBCode>
					<xsl:value-of select="'Bloomberg Ticker'"/>
				</BBCode>

				<!--2-->
				<Sedol>
					<xsl:value-of select="'Sedol'"/>
				</Sedol>

				<!--3-->
				<Side>
					<xsl:value-of select="'Side'"/>
				</Side>

				<!--4-->
				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<!--5-->
				<PrimeBroker>
					<xsl:value-of select="'Prime Broker'"/>
				</PrimeBroker>

				<!--6-->
				<FundAccountNumber>
					<xsl:value-of select="'Fund Account Number'"/>
				</FundAccountNumber>

				<!--7-->
				<ExecutingBroker>
					<xsl:value-of select="'Executing Broker'"/>
				</ExecutingBroker>

				<!--8-->
				<QuantityTraded>
					<xsl:value-of select="'Quantity Traded'"/>
				</QuantityTraded>

				<!--9-->
				<Price>
					<xsl:value-of select="'Average Trade Price'"/>
				</Price>


				<AllocatedQuantity>
					<xsl:value-of select="'Allocated Quantity'"/>
				</AllocatedQuantity>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>   

			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty = 'DBElec' and CurrencySymbol = 'AUD']">
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

					<!--1-->
					<BBCode>
						<xsl:value-of select="BBCode"/>
					</BBCode>

					<!--2-->
					<Sedol>
						<xsl:value-of select="SEDOL"/>
					</Sedol>

					<!--3-->
					<Side>
						<xsl:value-of select="Side"/>
					</Side>

					<!--4-->
					<TradeDate>
						<!--<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>-->
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<!--5-->
					<FundAccountNumber>
						<xsl:value-of select="AccountNo"/>
					</FundAccountNumber>

					<!--6-->
					<xsl:variable name="varFundName">
						<xsl:value-of select="FundName"/>
					</xsl:variable>

					<xsl:variable name="varPB">
						<xsl:value-of select="document('../ReconMappingXml/Omgeo_BrokerMatch.xml')/BrokerMapping/PB[@Name='SENSATO']/BrokerData[@PranaFundName = $varFundName]/@PB"/>
					</xsl:variable>
					
					<PrimeBroker>
						<xsl:choose>
							<xsl:when test="$varPB != ''">
								<xsl:value-of select ="$varPB"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PrimeBroker>

					<!--7-->
					
					
					
					<ExecutingBroker>
						<xsl:choose>
						<xsl:when test ="CounterParty='DBPrg' or CounterParty='DBElec'">
							<xsl:value-of select ="'DBAB'"/>
						</xsl:when>
						</xsl:choose>
			      </ExecutingBroker>

					<!--8-->
					<QuantityTraded>
						<xsl:value-of select="ExecutedQty"/>
					</QuantityTraded>

					<!--9-->
					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<AllocatedQuantity>
						<xsl:value-of select="AllocatedQty"/>
					</AllocatedQuantity>
					
				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

