<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/ThirdPartyOne_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test="AssetID = 2">

					<ThirdPartyFlatFileDetail>

						<RowHeader>
							<xsl:value-of select ="'false'"/>
						</RowHeader>

						<TaxlotState>
							<xsl:value-of select ="TaxLotState"/>
						</TaxlotState>

						<xsl:choose>
							<xsl:when test ="FundMappedName='44DM'">
								<ACCOUNT>
									<xsl:value-of select="concat(FundMappedName,'1709')"/>
								</ACCOUNT>
							</xsl:when>
							<xsl:when test ="FundMappedName='4YW1'">
								<ACCOUNT>
									<xsl:value-of select="concat(FundMappedName,'1209')"/>
								</ACCOUNT>
							</xsl:when>
							<xsl:otherwise>
								<ACCOUNT>
									<xsl:value-of select="FundMappedName"/>
								</ACCOUNT>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varSymbolbefSpace">
							<xsl:value-of select ="substring-before(Symbol,' ')"/>
						</xsl:variable>

						<xsl:variable name ="varSymbolafterSpace">
							<xsl:value-of select ="substring-after(Symbol,' ')"/>
						</xsl:variable >

						<SYMBOL>
							<xsl:value-of select ="concat($varSymbolbefSpace,$varSymbolafterSpace)"/>
						</SYMBOL>

						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<SIDE>
									<xsl:value-of select="'B'"/>
								</SIDE>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<SIDE>
									<xsl:value-of select="'BC'"/>
								</SIDE>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<SIDE>
									<xsl:value-of select="'S'"/>
								</SIDE>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<SIDE>
									<xsl:value-of select="'SS'"/>
								</SIDE>
							</xsl:when>
							<xsl:otherwise>
								<SIDE>
									<xsl:value-of select="''"/>
								</SIDE>
							</xsl:otherwise>
						</xsl:choose>

						<QUANTITY>
							<xsl:value-of select="AllocatedQty"/>
						</QUANTITY>

						<PRICE>
							<xsl:value-of select="AveragePrice"/>
						</PRICE>

						<DONEWAYCOMM>
							<xsl:value-of select ="CommissionCharged + TaxOnCommissions + OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>
						</DONEWAYCOMM>

						<EXCHANGE>
							<xsl:value-of select ="'Q'"/>
						</EXCHANGE>

						<xsl:choose>
							<xsl:when test="Side = Side = 'Buy' or 'Buy to Open' or Side = 'Sell to Open' or Side = 'Sell short'">
								<CMTAOPENCLOSE>
									<xsl:value-of select ="'O'"/>
								</CMTAOPENCLOSE>
							</xsl:when>
							<xsl:when test="Side = 'Buy to Close' or Side = 'Buy to Cover' or Side = 'Sell to Close' or Side = 'Sell'">
								<CMTAOPENCLOSE>
									<xsl:value-of select ="'C'"/>
								</CMTAOPENCLOSE>
							</xsl:when>
							<xsl:otherwise>
								<CMTAOPENCLOSE>
									<xsl:value-of select ="''"/>
								</CMTAOPENCLOSE>
							</xsl:otherwise>
						</xsl:choose>

						<!--<OPTIONFORMAT>
							<xsl:value-of select ="'S'"/>
						</OPTIONFORMAT>

						<OPTIONDONEWAY>
							<xsl:value-of select ="'Y'"/>
						</OPTIONDONEWAY>-->

						<!-- it is mandatory filed, since it is not see on the UI but use in the bachround code-->
						<xsl:for-each select="EntityID">
							<EntityID>
								<xsl:value-of select="."/>
							</EntityID>
						</xsl:for-each>

					</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
