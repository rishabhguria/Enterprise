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

				<AccountNumber>
					<xsl:value-of select="'Account Number'"/>
				</AccountNumber>

				<OrderSide>
					<xsl:value-of select="'Order Side'"/>
				</OrderSide>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Ticker>
					<xsl:value-of select="'Ticker'"/>
				</Ticker>

				<BlankColumn1>
					<xsl:value-of select="'BlankColumn1'"/>
				</BlankColumn1>

				<BlankColumn2>
					<xsl:value-of select="'BlankColumn2'"/>
				</BlankColumn2>

				<BlankColumn3>
					<xsl:value-of select="'BlankColumn3'"/>
				</BlankColumn3>

				<Defaultvalue>
					<xsl:value-of select="'Default value'"/>
				</Defaultvalue>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
			
					<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'False'"/>
						</RowHeader>

						<!--for system use only-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'False'"/>
						</IsCaptionChangeRequired>

						<!--for system internal use-->
						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<AccountNumber>
							<xsl:value-of select="AccountNo"/>
						</AccountNumber>						

						<OrderSide>
							<xsl:value-of select="substring(Side,1,1)"/>
						</OrderSide>

						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>

						<Ticker>
							<xsl:value-of select="Symbol"/>
						</Ticker>

						<BlankColumn1>
							<xsl:value-of select="''"/>
						</BlankColumn1>

						<BlankColumn2>
							<xsl:value-of select="''"/>
						</BlankColumn2>

						<BlankColumn3>
							<xsl:value-of select="''"/>
						</BlankColumn3>

						<Defaultvalue>
							<xsl:value-of select="'E'"/>
						</Defaultvalue>

						<!-- system use only-->
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>
					</ThirdPartyFlatFileDetail>
			
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
