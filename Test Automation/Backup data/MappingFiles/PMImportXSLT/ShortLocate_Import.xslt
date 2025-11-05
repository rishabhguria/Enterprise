<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
		
			<xsl:for-each select ="//PositionMaster">
			<xsl:if test="COL3='Locate Required'">
					<PositionMaster>
						<Ticker>
							<xsl:value-of select="COL1"/>
						</Ticker>
						
						<TradeQuantity>
							<xsl:value-of select="0"/>
						</TradeQuantity>
						
						<BorrowSharesAvailable>
							<xsl:value-of select="COL5"/>
						</BorrowSharesAvailable>
						
						<xsl:variable name="varBorrowRate">
							<xsl:choose>
								<xsl:when test ="contains(COL7,'BPS')">
								<xsl:value-of select="substring-before(COL7,' ')"/>
								</xsl:when>
								<xsl:when test ="contains(COL7,'%')">
								<xsl:value-of select="substring-before(COL7,'%') *100"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
		   
						<BorrowRate>
							<xsl:value-of select="$varBorrowRate"/>
						</BorrowRate>
						
						<BorrowerId>
							<xsl:value-of select="COL6"/>
						</BorrowerId>
						
						<StatusSource>
							<xsl:value-of select="'API'"/>
						</StatusSource>				
						
					</PositionMaster>
					</xsl:if>
			</xsl:for-each>
	
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
