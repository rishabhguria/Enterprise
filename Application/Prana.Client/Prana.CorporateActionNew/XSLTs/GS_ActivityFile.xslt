<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL1 != 'Account Number'">

					<!--TABLE-->
					<PositionMaster>

						<!--<FundName>
							<xsl:value-of select="COL1"/>
						</FundName>-->
						<OrigSymbol>
							<xsl:value-of select="COL3"/>
						</OrigSymbol>
						<ExDivDate>
							<xsl:value-of select="COL4"/>
						</ExDivDate>
						<RecordDate>
							<xsl:value-of select="COL5"/>
						</RecordDate>
						<DivPayoutDate>
							<xsl:value-of select="COL6"/>
						</DivPayoutDate>
						<!--<Position>
							<xsl:value-of select="COL7"/>
						</Position>-->
						<DivRate>
							<xsl:value-of select="COL8"/>
						</DivRate>
						<!--<PaybleCurrency>
							<xsl:value-of select="COL13"/>
						</PaybleCurrency>
						<ActivityType>
							<xsl:value-of select="COL15"/>
						</ActivityType>-->

					</PositionMaster>
					
				</xsl:if >
			</xsl:for-each>			
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
