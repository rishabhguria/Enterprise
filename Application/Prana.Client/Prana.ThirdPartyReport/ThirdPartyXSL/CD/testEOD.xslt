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

				<SIDE>
					<xsl:value-of select="'Side'"/>
				</SIDE>

				<QUANTITY>
					<xsl:value-of select="'Quantity'"/>
				</QUANTITY>

				<SYMBOL>
					<xsl:value-of select="'Symbol'"/>
				</SYMBOL>

				<PRICE>
					<xsl:value-of select="'Price'"/>
				</PRICE>			

				<!-- system use only-->
				
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test="Asset='Equity'">
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

						<SIDE>
							<xsl:value-of select="Side"/>
						</SIDE>

						<QUANTITY>
							<xsl:value-of select="AllocatedQty"/>
						</QUANTITY>

						<SYMBOL>
							<xsl:value-of select="Symbol"/>
						</SYMBOL>

						<PRICE>
							<xsl:value-of select="AveragePrice"/>
						</PRICE>

						

						<!-- system use only-->
					
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>
					</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
