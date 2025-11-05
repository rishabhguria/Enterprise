<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="normalize-space(COL1) != 'Account Name' and number(COL16) and normalize-space(COL1)!=' '">
					<PositionMaster>

						<AccountName>
							<xsl:value-of select="COL1"/>
						</AccountName>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="COL3"/>
						</LocalCurrency>

						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<xsl:choose>
						
							<xsl:when test ="boolean(number(COL16))">
								<CashValueLocal>
									<xsl:value-of select="COL16"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select= '0' />
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >

						<Date>
							<xsl:value-of select="''"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
