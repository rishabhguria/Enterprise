<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="PositionMaster">
				<xsl:if test="COL1 != 'Account Number'">

					<!--TABLE-->
					<PositionMaster>

						<!--FundNameSection -->

						<Symbol>
							<xsl:value-of select="COL4"/>
						</Symbol>
						<Dividend>
							<xsl:value-of select="translate(COL9,'N/A','0')"/>
						</Dividend>
						<PayoutDate>
							<xsl:value-of select="COL11"/>
						</PayoutDate>
						<ExDate>
							<xsl:value-of select="COL11"/>
						</ExDate>

					</PositionMaster>
					
				</xsl:if >
			</xsl:for-each>			
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
